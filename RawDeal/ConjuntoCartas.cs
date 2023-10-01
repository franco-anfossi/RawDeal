namespace RawDeal;

public class ConjuntoCartas
{
    public List<Carta> CartasPosibles { get; private set; }
    public List<Jugador> SuperstarsPosibles { get; private set; }
    
    public ConjuntoCartas(List<Carta> cartas, List<Jugador> superstars)
    {
        CartasPosibles = cartas;
        SuperstarsPosibles = superstars;
    }
}

