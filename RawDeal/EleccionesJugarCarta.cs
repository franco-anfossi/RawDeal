using RawDealView.Formatters;
using RawDealView;

namespace RawDeal;

public static class EleccionesJugarCarta
{
    private static List<IViewablePlayInfo> _jugadasPosiblesNoFormateadas = new();
    private static List<string> _jugadasPosiblesFormateadas = new();
    private static View _view;
    private static Superstar _jugadorEnTurno;
    
    public static void CrearJugadas(Superstar jugador)
    {
        _jugadorEnTurno = jugador;
        List<Carta> cartasJugables = jugador.RevisarCartasJugables();
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
        string jugadaElegida = PreguntarAlUsuarioParaJugarCarta();
        UsuarioIntentaJugarCarta(jugadaElegida);
        SeJuegaLaCartaExitosamente();
    }
    
    private static string PreguntarAlUsuarioParaJugarCarta()
    {
        int numeroJugadaSeleccionada = _view.AskUserToSelectAPlay(_jugadasPosiblesFormateadas);
        return _jugadasPosiblesFormateadas[numeroJugadaSeleccionada];
    }

    private static void UsuarioIntentaJugarCarta(string jugadaElegida)
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_jugadorEnTurno.Name, jugadaElegida);
    }

    private static void SeJuegaLaCartaExitosamente()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private static void SeHaceDañoAlOponente()
    {
        //_view.SayThatSuperstarWillTakeSomeDamage();
    }

}