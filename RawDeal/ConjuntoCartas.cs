namespace RawDeal;

public class ConjuntoCartas
{
    public List<Carta> Cartas { get; }
    public List<Superstar> Superstars { get; }

    public ConjuntoCartas(List<Carta> cartas, List<Superstar> superstars)
    {
        Cartas = cartas;
        Superstars = superstars;
    }
}

