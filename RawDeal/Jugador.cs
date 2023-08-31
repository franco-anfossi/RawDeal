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
    public List<Carta> Hand;

    public Jugador(Mazo mazo)
    {
        MiMazo = mazo;
        MiSuperstar = mazo.Superstar;
        Arsenal = mazo.Cartas;
        Name = MiSuperstar.Name;
        HandSize = MiSuperstar.HandSize;
        ActualizarDatos();
    }

    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(Name, Fortitude, Hand.Count, Arsenal.Count);
    }
}