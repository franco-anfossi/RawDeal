using RawDealView;

namespace RawDeal;

public class Jugador
{
    public Mazo MiMazo { get; }
    public Superstar MiSuperstar { get; }
    public PlayerInfo DatosJugador { get; private set; }

    private List<Carta> _arsenal;
    private int _fortitude = 0;
    private string _name;
    private int _handSize;
    private List<Carta> _hand = new();
    private List<Carta> _ringside = new();

    public Jugador(Mazo mazo)
    {
        MiMazo = mazo;
        MiSuperstar = mazo.SuperstarDelMazo;
        _arsenal = mazo.CartasDelMazo;
        _name = MiSuperstar.Name;
        _handSize = MiSuperstar.HandSize;
        
        _arsenal.Shuffle();
        ActualizarDatos();
    }

    public void SacarCartasInicio()
    {
        for (int i = 0; i < _handSize; i++)
            SacarCarta();
        ActualizarDatos();
    }

    public void SacarCarta()
    {
        int ultimaCarta = _arsenal.Count - 1;  
        if (ultimaCarta >= 0)
        {
            _hand.Add(_arsenal[ultimaCarta]);
            _arsenal.RemoveAt(ultimaCarta);
        }
        ActualizarDatos();
    }
    
    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(_name, _fortitude, _hand.Count, _arsenal.Count);
    }
}