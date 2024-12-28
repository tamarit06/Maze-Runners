using Spectre.Console;
using System;
using System.Collections.Generic;

public class Laberinto
{
    public Celda[,] tablero;
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    private Random rand = new Random();

    

    public Laberinto(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        InicializarTablero();
        GenerarLaberintoConPrim();
        AgregarTrampas(8);
        AgregarPescado(7);
    }

    private void InicializarTablero()
    {
        tablero = new Celda[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                tablero[i, j] = new Celda();
            }
        }
    }

    private void GenerarLaberintoConPrim()
    {
        List<(int, int)> celdasAbiertas = new List<(int, int)>();
        int[] dx = { -1, 1, 0, 0 };  
        int[] dy = { 0, 0, -1, 1 };

        int inicioX = 1, inicioY = 1;
        tablero[inicioX, inicioY].ConvertirEnCamino(); 
        celdasAbiertas.Add((inicioX, inicioY));

        while (celdasAbiertas.Count > 0)
        {
            var (x, y) = celdasAbiertas[rand.Next(celdasAbiertas.Count)];
            List<(int, int)> celdasAdyacentes = new List<(int, int)>();
            for (int i = 0; i < 4; i++)
            {
                int nuevoX = x + dx[i] * 2;
                int nuevoY = y + dy[i] * 2;
                if (EsValido(nuevoX, nuevoY) && tablero[nuevoX, nuevoY].Tipo==Celda.TipoCelda.Pared)
                {
                    celdasAdyacentes.Add((nuevoX, nuevoY));
                }
            }
            if (celdasAdyacentes.Count > 0)
            {
                var (nx, ny) = celdasAdyacentes[rand.Next(celdasAdyacentes.Count)];
                tablero[nx, ny].ConvertirEnCamino();
                tablero[(x + nx) / 2, (y + ny) / 2].ConvertirEnCamino(); 
                celdasAbiertas.Add((nx, ny));
            }
            else
            {
                celdasAbiertas.Remove((x, y));
            }
        }
    }

    private List<(int x,int y)> PosicionesCamino()
    {
        List<(int x, int y)> posicionesCamino = new List<(int x, int y)>();

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (tablero[i, j].Tipo == Celda.TipoCelda.Camino && i!=1 && j!=1 && i!=19 && j!=19)
                {
                    posicionesCamino.Add((i, j));
                }
            }
        }

        return posicionesCamino;
    }

    private void AgregarTrampas(int cantidad)
   {

        List<(int x, int y)> posicionesCamino = PosicionesCamino();
        // Seleccionar aleatoriamente posiciones para las trampas
        for (int i = 0; i <=cantidad; i++)
        {
            var posicionTrampa = posicionesCamino[rand.Next(posicionesCamino.Count)];

            // Seleccionar un tipo de trampa aleatorio
            int tipoTrampa = rand.Next(1, 4); // Valores entre 1 y 3
            switch (tipoTrampa)
            {
                case 1:
                    tablero[posicionTrampa.x, posicionTrampa.y].AsignarTrampa(new TrampaDevolver(5)); // Devolver 5 celdas
                    break;
                case 2:
                    tablero[posicionTrampa.x, posicionTrampa.y].AsignarTrampa(new TrampaQuitarTurno());
                    break;
                case 3:
                    tablero[posicionTrampa.x, posicionTrampa.y].AsignarTrampa(new TrampaAnularHabilidad());
                    break; 
            }

            posicionesCamino.Remove(posicionTrampa); // Eliminar la posición para no repetir
        }
    }  

    private void AgregarPescado(int cantidad)
    {

         List<(int x, int y)> posicionesCamino = PosicionesCamino();
        for (int i = 0; i <=cantidad; i++)
        {
            var posicionPescado = posicionesCamino[rand.Next(posicionesCamino.Count)];
        
            tablero[posicionPescado.x, posicionPescado.y].ConvertirEnPescado(); // Devolver 5 celdas

            posicionesCamino.Remove(posicionPescado); // Eliminar la posición para no repetir
        }
    }

    


   public void MostrarLaberinto()
   { 
        
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                 if(tablero[i,j].HayJugador && tablero[i,j].jugador.representacionEnConsola == 1 && tablero[i,j].Tipo!=Celda.TipoCelda.Pared )
                {
                    AnsiConsole.Markup("[blue]😾[/]");
                }
                 else if(tablero[i,j].HayJugador && tablero[i,j].jugador.representacionEnConsola == 2  && tablero[i,j].Tipo!=Celda.TipoCelda.Pared)
                {
                    AnsiConsole.Markup("[blue]😸[/]");
                }

               else if (tablero[i, j].Tipo==Celda.TipoCelda.Camino)
                {
                    // Imprimir espacio vacío
                    AnsiConsole.Markup("[green]  [/]");
                }
                
                else if (tablero[i,j].Tipo==Celda.TipoCelda.Pared)
                {
                    // Imprimir bloque del laberinto en verde
                    AnsiConsole.Markup("[green]🧱[/]");
                }
                
                else if (tablero[i, j].Tipo == Celda.TipoCelda.Trampa)
                { 
                    if (tablero[i, j].TrampaAsociada is TrampaDevolver)
                    {
                        AnsiConsole.Markup("[red]❌[/]"); 
                    }
                    else if (tablero[i, j].TrampaAsociada is TrampaAnularHabilidad)
                    {
                        AnsiConsole.Markup("[red]🛑[/]"); // A para Anular Habilidad
                    }
                    else if (tablero[i, j].TrampaAsociada is TrampaQuitarTurno)
                    {
                        AnsiConsole.Markup("[red]🚫[/]"); // T para Quitar Turno
                    }
                }

                else if (tablero[i,j].Tipo==Celda.TipoCelda.Pescado)
                {
                    AnsiConsole.Markup("[red]🐠[/]");
                }

                
            }
            AnsiConsole.WriteLine(""); 
        }
   }

    public bool EsValido(int x, int y)
    {
        return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
    }

    public Celda[,] Tablero => tablero;
}
