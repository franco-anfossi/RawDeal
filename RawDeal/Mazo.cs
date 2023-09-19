using RawDealView.Formatters;

namespace RawDeal;

public class Mazo
{
    private Superstar _superstarDuenoDelMazo;
    private List<IViewableCardInfo> _cartasDelMazo;
    private ConjuntoCartas _conjuntoCartas;
    
    public Superstar SuperstarDelMazo => _superstarDuenoDelMazo;
    public List<IViewableCardInfo> CartasDelMazo => _cartasDelMazo;
    
    public Mazo(string[] listaMazo, ConjuntoCartas conjunto)
    {
        _cartasDelMazo = new List<IViewableCardInfo>();
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
                _superstarDuenoDelMazo = (Superstar)superstar.Clonar();
        }
    }

    private void AgregarCartaEquivalenteAlMazo(string[] listaMazo)
    {
        foreach (var fila in listaMazo)
        foreach (var carta in _conjuntoCartas.CartasPosibles)
        {
            if (fila.Trim() == carta.Title)
            {
                Carta copiaCarta = (Carta)carta.Clonar();
                _cartasDelMazo.Add(copiaCarta);
            }
        }
    }

    private void SetMazoParaElSuperstarDueno()
    {
        _superstarDuenoDelMazo.InicializacionDeAtributos(_cartasDelMazo);
    }
    
    
}