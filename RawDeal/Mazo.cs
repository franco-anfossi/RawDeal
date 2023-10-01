using RawDealView.Formatters;

namespace RawDeal;

public class Mazo
{
    private Jugador _jugadorDuenoDelMazo;
    private List<IViewableCardInfo> _cartasDelMazo;
    private ConjuntoCartas _conjuntoDeCartas;
    
    public Jugador JugadorDelMazo => _jugadorDuenoDelMazo;
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
            if (superstar.CompararNombres(nombreSuperstarDelMazo))
            {
                _jugadorDuenoDelMazo = (Jugador)superstar.Clonar();
            }
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
        _jugadorDuenoDelMazo.InicializarLosAtributosNecesarios(_cartasDelMazo);
    }

    private void AgregarCopiaDeLaClaseBaseDeLaCarta(Carta carta)
    {
        Carta copiaCarta = (Carta)carta.Clonar();
        _cartasDelMazo.Add(copiaCarta);
    }
    
}