public class Celda
{
    public enum TipoCelda
    {
        Camino,
        Pared,
        Trampa,
        Obstaculo
    }

    public TipoCelda Tipo { get; set; }
    public Trampa TrampaAsociada {get; set; }

    public bool HayJugador;
    public Jugador jugador { get; set; }

    public Celda()
    {
        Tipo = TipoCelda.Pared; 
        TrampaAsociada= null;
        HayJugador=false;
    }

    public void ConvertirEnCamino()
    {
        Tipo = TipoCelda.Camino;
        TrampaAsociada= null;
    }

    public void ConvertirEnPared()
    {
        Tipo = TipoCelda.Pared;
        TrampaAsociada=null;
    }

    public void ConvertirEnTrampa(Trampa trampa)
    {
        Tipo = TipoCelda.Trampa;
        TrampaAsociada=trampa;
    }

    public void AsignarTrampa(Trampa trampa)
    {
        Tipo=TipoCelda.Trampa;
        TrampaAsociada=trampa;
    }

    
}
