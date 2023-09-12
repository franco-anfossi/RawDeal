namespace RawDeal;

public class Mazo
{
    public List<Superstar> SuperstarsInicial { get; }
    public Superstar Superstar { get; private set; }
    public List<Carta> Cartas { get; }
    private ConjuntoCartas _conjunto;

    public Mazo(string[] listaMazo, ConjuntoCartas conjunto)
    {
        SuperstarsInicial = new List<Superstar>();
        Cartas = new List<Carta>();
        _conjunto = conjunto;

        RevisarSiCartaEsSuperstar(listaMazo);
        AgregarCartaEquivalenteAlMazo(listaMazo);
    }

    public void RevisarSiCartaEsSuperstar(string[] listaMazo)
    {
        foreach (string fila in listaMazo)
        {
            if (fila.Contains(" (Superstar Card)"))
                AgregarCartaSuperstarEquivalenteAlMazo(fila);
        }
    }

    private void AgregarCartaSuperstarEquivalenteAlMazo(string fila)
    {
        string nombreSuperstar = fila.Replace("(Superstar Card)", "").Trim();
        foreach (var superstar in _conjunto.Superstars)
        {
            if (superstar.Name == nombreSuperstar)
                SuperstarsInicial.Add(superstar);
        }
    }

    public void AgregarCartaEquivalenteAlMazo(string[] listaMazo)
    {
        foreach (var fila in listaMazo)
        foreach (var carta in _conjunto.Cartas)
        {
            if (fila.Trim() == carta.Title)
                Cartas.Add(carta);
        }
    }

    public void AgregarSuperstarComoAtributo()
    {
        Superstar = SuperstarsInicial[0];
    }
}