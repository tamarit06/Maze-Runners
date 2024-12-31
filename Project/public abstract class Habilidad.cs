public abstract class Habilidad
{
    public string Nombre { get; set; }
    public int TiempoEnfriamiento { get; set; } // Tiempo de enfriamiento en turnos
    public int TurnosRestantes { get; set; }
     
    public Habilidad()
    {
        TurnosRestantes=0;
    }
    public abstract void Activar(Jugador jugador, Laberinto laberinto);

    public void ActualizarEnfriamiento()
    {
        if (TurnosRestantes > 0)
        {
            TurnosRestantes--;
        }
    }

    public bool PuedeActivar()
    {
        return TurnosRestantes == 0; // La habilidad se puede activar si no hay turnos restantes
    }
}

public class HabilidadDuplicarPuntos : Habilidad
{
    public HabilidadDuplicarPuntos()
    {
        Nombre = "Duplicar Puntos";
        TiempoEnfriamiento=6;
    }

    public override void Activar(Jugador jugador, Laberinto laberinto)
    {
       if (PuedeActivar())
        {
            TurnosRestantes = TiempoEnfriamiento; // Reiniciar el tiempo de enfriamiento
        }
    }
}

public class HabilidadSuperVelocidad : Habilidad
{
    int velocidadOriginal;

    public HabilidadSuperVelocidad()
    {
        Nombre = "Supervelocidad";
        TiempoEnfriamiento=3;
    }

    public override void Activar(Jugador jugador, Laberinto laberinto)
    {
        velocidadOriginal=jugador.FichaElegida.Velocidad;
        if (PuedeActivar())
        {
            jugador.FichaElegida.Velocidad+=5;
            TurnosRestantes = TiempoEnfriamiento; // Reiniciar el tiempo de enfriamiento
        }
        
    }

     public void RestablecerVelocidad(Jugador jugador)
    {
        jugador.FichaElegida.Velocidad = velocidadOriginal; //esto no pincha revisa
    }
}

public class HabilidadInmunidad : Habilidad
{
    public HabilidadInmunidad()
    {
        Nombre = "Inmunidad a trampa";
        TiempoEnfriamiento=4;
    }

    public override void Activar(Jugador jugador, Laberinto laberinto)
    {
       if (PuedeActivar())
       {
        jugador.FichaElegida.EsInmune=true;
        TurnosRestantes = TiempoEnfriamiento; // Reiniciar el tiempo de enfriamiento
       }
    }

    public void QuitarInmunidad(Jugador jugador)
    {
       jugador.FichaElegida.EsInmune=false;
    }
}

public class HabilidadTeletransportacion : Habilidad
{
    private Random rand = new Random();
    List<(int x,int y)> posicionesCamino;
    public HabilidadTeletransportacion()
    {
        Nombre = "Teletransportación Total";
        TiempoEnfriamiento=4;
    }

    public override void Activar(Jugador jugador, Laberinto laberinto)
    {
        if (PuedeActivar())
        {
            posicionesCamino=laberinto.PosicionesCamino();
            var nuevaPos = posicionesCamino[rand.Next(posicionesCamino.Count)];
    
         // Actualizar la posición del jugador
         laberinto.tablero[jugador.PositionX, jugador.PositionY].HayJugador = false;
         laberinto.tablero[jugador.PositionX, jugador.PositionY].jugador = null;

        jugador.PositionX = nuevaPos.x;
        jugador.PositionY = nuevaPos.y;

        laberinto.tablero[jugador.PositionX, jugador.PositionY].HayJugador = true;
        laberinto.tablero[jugador.PositionX, jugador.PositionY].jugador = jugador;

        TurnosRestantes = TiempoEnfriamiento; // Reiniciar el tiempo de enfriamiento
        }
    }
}
public class HabilidadAtrabezarPared : Habilidad
{
    public HabilidadAtrabezarPared()
    {
        Nombre = "Atrabezar Pared";
        TiempoEnfriamiento=3;
    }

    public override void Activar(Jugador jugador, Laberinto laberinto)
    {
       if (PuedeActivar())
       {
        
        TurnosRestantes = TiempoEnfriamiento; // Reiniciar el tiempo de enfriamiento
       }

    }
}