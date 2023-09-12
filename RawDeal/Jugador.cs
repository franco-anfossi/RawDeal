using RawDealView;

namespace RawDeal;

public class Jugador
{
    public Mazo MiMazo { get; }
    public Superstar MiSuperstar;
    public List<Carta> Arsenal;
    public PlayerInfo DatosJugador;

    public int Fortitude = 0;
    public string Name;
    public int HandSize;
    public List<Carta> Hand = new();
    public List<Carta> Ringside = new();

    public Jugador(Mazo mazo)
    {
        MiMazo = mazo;
        MiSuperstar = mazo.Superstar;
        Arsenal = mazo.Cartas;
        Name = MiSuperstar.Name;
        HandSize = MiSuperstar.HandSize;
        
        Arsenal.Shuffle();
        ActualizarDatos();
    }

    public void SacarCartasInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        ActualizarDatos();
    }

    public void SacarCarta()
    {
        int ultimaCarta = Arsenal.Count - 1;  
        if (ultimaCarta >= 0)
        {
            Hand.Add(Arsenal[ultimaCarta]);
            Arsenal.RemoveAt(ultimaCarta);
        }
        ActualizarDatos();
    }
    
    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(Name, Fortitude, Hand.Count, Arsenal.Count);
    }
}