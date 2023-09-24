namespace RawDeal;

public class ConjuntoCartas
{
    public List<Carta> CartasPosibles { get; private set; }
    public List<Superstar> SuperstarsPosibles { get; private set; }
    
    public ConjuntoCartas(List<Carta> cartas, List<Superstar> superstars)
    {
        CartasPosibles = cartas;
        SuperstarsPosibles = superstars;
    }
}

