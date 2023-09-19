using RawDealView.Formatters;
using RawDealView;

namespace RawDeal;

public static class EleccionesJugarCarta
{
    private static List<IViewablePlayInfo> _jugadasPosiblesNoFormateadas = new();
    private static List<string> _jugadasPosiblesFormateadas = new();
    private static string _jugadaElegidaFormateada;
    private static IViewablePlayInfo _jugadaElegidaNoFormateada;
    private static View _view;
    private static Superstar _jugadorEnTurno;
    private static Superstar _jugadorOponente;
    
    public static void CrearJugadas(Superstar jugadorEnTurno, Superstar jugadorOponente)
    {
        _jugadasPosiblesNoFormateadas = new List<IViewablePlayInfo>();
        _jugadasPosiblesFormateadas = new List<string>();
        _jugadorOponente = jugadorOponente;
        _jugadorEnTurno = jugadorEnTurno;
        List<IViewableCardInfo> cartasJugables = jugadorEnTurno.RevisarCartasJugables();
        foreach (var cartaJugable in cartasJugables)
        {
            string cardTypeMayusculas = cartaJugable.Types[0].ToUpper();
            Jugada jugadaPosible = new Jugada(cartaJugable, cardTypeMayusculas);
            _jugadasPosiblesNoFormateadas.Add(jugadaPosible);
        }
    }

    public static void FormatearJugada()
    {
        foreach (var jugadaPosible in _jugadasPosiblesNoFormateadas)
        {
            string jugadaFormateada = Formatter.PlayToString(jugadaPosible);
            _jugadasPosiblesFormateadas.Add(jugadaFormateada);
        }
    }

    public static void ComenzarProcesoDeElecciones(View view)
    {
        _view = view;
        PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea();
        UsuarioIntentaJugarCarta();
        SeJuegaLaCartaExitosamente();
        SeHaceDanoAlOponente();
    }
    
    private static void PreguntarAlUsuarioParaJugarCartaYPonerlaEnRingArea()
    { 
        int numeroJugadaSeleccionada = _view.AskUserToSelectAPlay(_jugadasPosiblesFormateadas);
        _jugadaElegidaFormateada = _jugadasPosiblesFormateadas[numeroJugadaSeleccionada];
        _jugadaElegidaNoFormateada = _jugadasPosiblesNoFormateadas[numeroJugadaSeleccionada]; 
        _jugadorEnTurno.PasarCartaDeManoARingArea(_jugadaElegidaNoFormateada.CardInfo);
    }

    private static void UsuarioIntentaJugarCarta()
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_jugadorEnTurno.Name, _jugadaElegidaFormateada);
    }

    private static void SeJuegaLaCartaExitosamente()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private static void SeHaceDanoAlOponente()
    {
        string nombreOponente = _jugadorOponente.Name;
        IViewableCardInfo cartaElegida = _jugadaElegidaNoFormateada.CardInfo;
        int danoDado = Convert.ToInt32(cartaElegida.Damage);
        _view.SayThatSuperstarWillTakeSomeDamage(nombreOponente, danoDado);
        _jugadorEnTurno.AgregarFortitudeSegunDano(danoDado);
        MostrarDanoHechoAlOponente(danoDado);
    }

    private static void MostrarDanoHechoAlOponente(int danoDado)
    {
        for (int iteracionDelDano = 1; iteracionDelDano <= danoDado; iteracionDelDano++)
        {
            IViewableCardInfo cartaExtraida = _jugadorOponente.PasarCartasDeArsenalARingside();
            string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);
                
            _view.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, iteracionDelDano, danoDado);
        }
    }

}