using RawDealView.Formatters;
using RawDealView;

namespace RawDeal;

public class EleccionesJugarCarta
{
    private List<IViewablePlayInfo> _jugadasPosiblesNoFormateadas = new();
    private List<string> _jugadasPosiblesFormateadas = new();
    private string _jugadaElegidaFormateada;
    private IViewablePlayInfo _jugadaElegidaNoFormateada;
    private View _view;
    private Superstar _jugadorEnTurno;
    private Superstar _jugadorOponente;
    private bool _continuarPartida = true;

    public EleccionesJugarCarta(Superstar jugadorEnTurno, Superstar jugadorOponente, View view)
    {
        _view = view;
        _jugadorEnTurno = jugadorEnTurno;
        _jugadorOponente = jugadorOponente;
        CrearJugadas();
        FormatearJugada();
    }

    public void CrearJugadas()
    {
        List<IViewableCardInfo> cartasJugables = _jugadorEnTurno.RevisarCartasJugables();
        foreach (var cartaJugable in cartasJugables)
        {
            string cardTypeMayusculas = cartaJugable.Types[0].ToUpper();
            if (cardTypeMayusculas != "REVERSAL")
            {
                Jugada jugadaPosible = new Jugada(cartaJugable, cardTypeMayusculas);
                _jugadasPosiblesNoFormateadas.Add(jugadaPosible);
            }
        }
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
        int numeroJugadaSeleccionada = _view.AskUserToSelectAPlay(_jugadasPosiblesFormateadas);
        if (_jugadasPosiblesFormateadas.Count >= 0 && numeroJugadaSeleccionada != -1)
        {
            PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea(numeroJugadaSeleccionada);
            UsuarioIntentaJugarCarta();
            SeJuegaLaCartaExitosamente();
            SeHaceDanoAlOponente();
        }

        return _continuarPartida;
    }

    private void PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea(int numeroJugadaSeleccionada)
    {
        _jugadaElegidaFormateada = _jugadasPosiblesFormateadas[numeroJugadaSeleccionada];
        _jugadaElegidaNoFormateada = _jugadasPosiblesNoFormateadas[numeroJugadaSeleccionada];
        _jugadorEnTurno.PasarCartaDeManoARingArea(_jugadaElegidaNoFormateada.CardInfo);
    }

    private void UsuarioIntentaJugarCarta()
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_jugadorEnTurno.Name, _jugadaElegidaFormateada);
    }

    private void SeJuegaLaCartaExitosamente()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private void SeHaceDanoAlOponente()
    {
        string nombreOponente = _jugadorOponente.Name;
        IViewableCardInfo cartaElegida = _jugadaElegidaNoFormateada.CardInfo;
        int danoDado = Convert.ToInt32(cartaElegida.Damage);
        int fortitudPorAgregar = danoDado;
        
        if (nombreOponente == "MANKIND")
            danoDado--;
        
        _view.SayThatSuperstarWillTakeSomeDamage(nombreOponente, danoDado);
        _jugadorEnTurno.AgregarFortitudeSegunDano(fortitudPorAgregar);
        MostrarDanoHechoAlOponente(danoDado);
    }

    private void MostrarDanoHechoAlOponente(int danoDado)
    {
        for (int iteracionDelDano = 1; iteracionDelDano <= danoDado; iteracionDelDano++)
        {
            if (_jugadorOponente.Arsenal.Count != 0)
            {
                IViewableCardInfo cartaExtraida = _jugadorOponente.PasarCartaDeArsenalARingside();
                string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);

                _view.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, iteracionDelDano, danoDado);
            }
            else
                _continuarPartida = false;
        }
    }
}