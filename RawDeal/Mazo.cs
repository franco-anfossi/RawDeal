using RawDealView.Formatters;

namespace RawDeal;

public class Mazo
{
    private Superstar _superstarDuenoDelMazo;
    private List<IViewableCardInfo> _cartasDelMazo;
    private ConjuntoCartas _conjuntoDeCartas;
    
    public Superstar SuperstarDelMazo => _superstarDuenoDelMazo;
    public List<IViewableCardInfo> CartasDelMazo => _cartasDelMazo;
    
    public Mazo(string[] mazoAbiertoDelArchivo, ConjuntoCartas conjuntoDeCartas)
    {
        _cartasDelMazo = new List<IViewableCardInfo>();
        _conjuntoDeCartas = conjuntoDeCartas;

        AgregarSuperstarEquivalenteAlMazo(mazoAbiertoDelArchivo);
        AgregarCartaEquivalenteAlMazo(mazoAbiertoDelArchivo);
        SetearMazoParaElSuperstarDueno();
    }

    private void AgregarSuperstarEquivalenteAlMazo(string[] mazoAbiertoDelArchivo)
    {
        string nombreSuperstarDelMazo = mazoAbiertoDelArchivo[0].Replace("(Superstar Card)", "").Trim();
        foreach (var superstar in _conjuntoDeCartas.SuperstarsPosibles)
        {
            if (superstar.Name == nombreSuperstarDelMazo) { _superstarDuenoDelMazo = (Superstar)superstar.Clonar(); }
        }
    }

    private void AgregarCartaEquivalenteAlMazo(string[] mazoAbieroDelArchivo)
    {
        foreach (var nombreDeLaCarta in mazoAbieroDelArchivo)
        foreach (var carta in _conjuntoDeCartas.CartasPosibles)
        {
            if (nombreDeLaCarta.Trim() == carta.Title)
                AgregarCopiaDeLaClaseBaseDeLaCarta(carta);
        }
    }

    private void SetearMazoParaElSuperstarDueno()
    {
        _superstarDuenoDelMazo.InicializarLosAtributosNecesarios(_cartasDelMazo);
    }

    private void AgregarCopiaDeLaClaseBaseDeLaCarta(Carta carta)
    {
        Carta copiaCarta = (Carta)carta.Clonar();
        _cartasDelMazo.Add(copiaCarta);
    }
    
}