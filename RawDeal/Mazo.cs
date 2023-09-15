namespace RawDeal;

public class Mazo
{
    private Superstar _superstarDelMazo;
    private List<Carta> _cartasDelMazo;
    private ConjuntoCartas _conjuntoCartas;
    
    public Superstar SuperstarDelMazo => _superstarDelMazo;
    public List<Carta> CartasDelMazo => _cartasDelMazo;
    
    

    public Mazo(string[] listaMazo, ConjuntoCartas conjunto)
    {
        _cartasDelMazo = new List<Carta>();
        _conjuntoCartas = conjunto;

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
        foreach (var superstar in _conjuntoCartas.SuperstarsPosibles)
        {
            if (superstar.Name == nombreSuperstar)
                _superstarDelMazo = superstar;
        }
    }

    public void AgregarCartaEquivalenteAlMazo(string[] listaMazo)
    {
        foreach (var fila in listaMazo)
        foreach (var carta in _conjuntoCartas.CartasPosibles)
        {
            if (fila.Trim() == carta.Title)
                _cartasDelMazo.Add(carta);
        }
    }
}