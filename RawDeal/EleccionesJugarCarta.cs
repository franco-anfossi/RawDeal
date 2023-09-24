using RawDealView.Formatters;
using RawDealView;

namespace RawDeal;

public class EleccionesJugarCarta
{
    private List<IViewablePlayInfo> _jugadasPosiblesNoFormateadas = new();
    private List<string> _jugadasPosiblesFormateadas = new();
    
    private IViewablePlayInfo _jugadaElegidaNoFormateada;
    private string _jugadaElegidaFormateada;
    
    private View _view;
    
    private Superstar _jugadorEnJuego;
    private Superstar _jugadorOponente;
    
    private bool _partidaActiva = true;

    public EleccionesJugarCarta(Superstar jugadorEnJuego, Superstar jugadorOponente, View view)
    {
        _view = view;
        _jugadorEnJuego = jugadorEnJuego;
        _jugadorOponente = jugadorOponente;
        CrearJugadas();
        FormatearJugada();
    }
    
    public void CrearJugadas()
    {
        List<IViewableCardInfo> cartasJugables = _jugadorEnJuego.RevisarCartasJugables();
        foreach (var cartaJugable in cartasJugables)
            AgregarJugadaSiEsQueNoEsDeTipoReversal(cartaJugable);
    }

    public void FormatearJugada()
    {
        foreach (var jugadaPosible in _jugadasPosiblesNoFormateadas)
        {
            string jugadaFormateada = Formatter.PlayToString(jugadaPosible);
            _jugadasPosiblesFormateadas.Add(jugadaFormateada);
        }
    }

    public bool ComenzarProcesoDeElecciones()
    {
        int numeroDeJugadaSeleccionada = _view.AskUserToSelectAPlay(_jugadasPosiblesFormateadas);
        if (_jugadasPosiblesFormateadas.Count >= 0 && numeroDeJugadaSeleccionada != -1)
        {
            PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea(numeroDeJugadaSeleccionada);
            IntentarJugarCarta();
            JugarCartaExitosamente();
            EjecutarDanoAlOponente();
        }

        return _partidaActiva;
    }

    private void AgregarJugadaSiEsQueNoEsDeTipoReversal(IViewableCardInfo cartaJugable)
    {
        string tipoDeCartaEnMayusculas = cartaJugable.Types[0].ToUpper();
        if (tipoDeCartaEnMayusculas != "REVERSAL")
        {
            Jugada jugadaPosible = new Jugada(cartaJugable, tipoDeCartaEnMayusculas);
            _jugadasPosiblesNoFormateadas.Add(jugadaPosible);
        }
    }
    private void PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea(int numeroJugadaSeleccionada)
    {
        _jugadaElegidaFormateada = _jugadasPosiblesFormateadas[numeroJugadaSeleccionada];
        _jugadaElegidaNoFormateada = _jugadasPosiblesNoFormateadas[numeroJugadaSeleccionada];
        _jugadorEnJuego.PasarCartaDeManoARingArea(_jugadaElegidaNoFormateada.CardInfo);
    }

    private void IntentarJugarCarta()
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_jugadorEnJuego.Name, _jugadaElegidaFormateada);
    }

    private void JugarCartaExitosamente()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private void EjecutarDanoAlOponente()
    {
        IViewableCardInfo cartaElegida = _jugadaElegidaNoFormateada.CardInfo;
        int danoHecho = Convert.ToInt32(cartaElegida.Damage);
        _jugadorEnJuego.HacerDanoAlOponente(danoHecho);
        if (_jugadorOponente.Name == "MANKIND") { danoHecho--; }
        
        MostrarDanoHechoAlOponente(danoHecho);
    }

    private void MostrarDanoHechoAlOponente(int danoTotalHecho)
    {
        for (int iteracionDelDano = 1; iteracionDelDano <= danoTotalHecho; iteracionDelDano++)
            DecidirSiHacerDanoONo(iteracionDelDano, danoTotalHecho);
    }

    private void DecidirSiHacerDanoONo(int iteracionDelDano, int danoTotalHecho)
    {
        if (!_jugadorOponente.ComprobarArsenalVacio())
            MostrarCartasPorElDanoHecho(iteracionDelDano, danoTotalHecho);
        else
            _partidaActiva = false;
    }

    private void MostrarCartasPorElDanoHecho(int iteracionDelDano, int danoTotalHecho)
    {
        IViewableCardInfo cartaExtraida = _jugadorOponente.PasarCartaDeArsenalARingside();
        string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);
        _view.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, iteracionDelDano, danoTotalHecho);
    }
}