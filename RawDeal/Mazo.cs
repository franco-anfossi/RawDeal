using RawDeal.Habilidades_Superstars;

namespace RawDeal;

public class Mazo
{
    private Superstar _superstarDuenoDelMazo;
    private List<Carta> _cartasDelMazo;
    private ConjuntoCartas _conjuntoCartas;
    
    public Superstar SuperstarDelMazo => _superstarDuenoDelMazo;
    public List<Carta> CartasDelMazo => _cartasDelMazo;
    
    public Mazo(string[] listaMazo, ConjuntoCartas conjunto)
    {
        _cartasDelMazo = new List<Carta>();
        _conjuntoCartas = conjunto;

        AgregarSuperstarEquivalenteAlMazo(listaMazo);
        AgregarCartaEquivalenteAlMazo(listaMazo);
        SetMazoParaElSuperstarDueno();
    }

    private void AgregarSuperstarEquivalenteAlMazo(string[] listaMazo)
    {
        string nombreSuperstar = listaMazo[0].Replace("(Superstar Card)", "").Trim();
        foreach (var superstar in _conjuntoCartas.SuperstarsPosibles)
        {
            if (superstar.Name == nombreSuperstar)
                _superstarDuenoDelMazo = superstar;
        }
    }

    private void AgregarCartaEquivalenteAlMazo(string[] listaMazo)
    {
        foreach (var fila in listaMazo)
        foreach (var carta in _conjuntoCartas.CartasPosibles)
        {
            if (fila.Trim() == carta.Title)
                _cartasDelMazo.Add(carta);
        }
    }

    private void SetMazoParaElSuperstarDueno()
    {
        _superstarDuenoDelMazo.IngresarMazo(_cartasDelMazo);
    }
    
    
}