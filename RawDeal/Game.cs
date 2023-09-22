using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private ConjuntoCartas _conjuntoCartas;
    private List<Superstar> _jugadores = new();
    private bool _continuarLoopPrincipal = true;
    private bool _continuarLoopElecciones = true;
    private int _numJugadorEnJuego = 0;
    private int _numJugadorOponente = 1;
    
    public Game(View view, string deckFolder)
    {
        var (superstarList, cardsList) = DeserializacionDeCartasYSuperstarsDesdeLosJson();
        _conjuntoCartas = new ConjuntoCartas(cardsList, superstarList);
        _view = view;
        _deckFolder = deckFolder;
    }

    private (List<Superstar>, List<Carta>) DeserializacionDeCartasYSuperstarsDesdeLosJson()
    {
        string archivoCartas = Path.Combine("data", "cards.json");
        string archivoSuperstars = Path.Combine("data", "superstar.json");
        
        Utils.AbrirArchivo(archivoSuperstars);
        var superstarList = Utils.DeserializacionSuperstar();
        
        Utils.AbrirArchivo(archivoCartas);
        var cardsList = Utils.DeserializacionCartas();

        return (superstarList, cardsList);
    }

    public void Play()
    {
        InicioEleccionMazo();
        if (_continuarLoopPrincipal)
        {
            ElegirJugadorInicial();
            ExtraerCartasInciales();
        }
        LoopPrincipalDelJuego();
    }
    // 1 Abstraccion
    private void InicioEleccionMazo()
    {
        for (int jugador = 0; jugador < 2; jugador++)
        {
            var listaMazo = AperturaDeMazoSegunArchivo();
            Mazo mazo = new Mazo(listaMazo, _conjuntoCartas);
            jugador = ValidarMazoParaContinuarJuego(mazo, jugador);
        }
    }

    private void LoopPrincipalDelJuego()
    {
        while (_continuarLoopPrincipal)
        {
            _continuarLoopElecciones = true;
            _view.SayThatATurnBegins(_jugadores[_numJugadorEnJuego].Name);
            SeHaceLaHabilidadEspecialAntesDeSacarCarta();
            _jugadores[_numJugadorEnJuego].CambiarVisibilidadDeElegirLaHabilidad();
            LoopEleccionesDelJuego();
        }
    }

    private void LoopEleccionesDelJuego()
    {
        while (_continuarLoopElecciones)
        {
            _view.ShowGameInfo(_jugadores[_numJugadorEnJuego].DatosJugador, _jugadores[_numJugadorOponente].DatosJugador);
            NextPlay eleccionDeLaPrimeraOpcion = CondicionParaMostrarLaEleccionDeHabilidad();
            EleccionesPosibles(eleccionDeLaPrimeraOpcion);
        }
    }

    private void SeHaceLaHabilidadEspecialAntesDeSacarCarta()
    {
        bool seSacaCarta = true;
        
        if (_jugadores[_numJugadorEnJuego].NoSePuedeEligirSiUsarLaHabilidad())
            seSacaCarta = _jugadores[_numJugadorEnJuego].HabilidadEspecial(_view, _jugadores[_numJugadorOponente]);
            
        if (seSacaCarta)
            _jugadores[_numJugadorEnJuego].SacarCarta();
    }

    private void EleccionesPosibles(NextPlay eleccionDeLaPrimeraOpcion)
    {
        if (eleccionDeLaPrimeraOpcion == NextPlay.ShowCards)
            SeleccionOpcionMostrarCartas();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.PlayCard)
            SeleccionOpcionJugarCartas();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.UseAbility)
            SeleccionOpcionJugarHabilidad();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.EndTurn)
            SeleccionOpcionFinalizarTurno();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.GiveUp)
            SeleccionOpcionRendirse();
    }

    private NextPlay CondicionParaMostrarLaEleccionDeHabilidad()
    {
        NextPlay primeraEleccionDelJuego;
        if (_jugadores[_numJugadorEnJuego].NoSePuedeEligirSiUsarLaHabilidad())
            primeraEleccionDelJuego = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        else
            primeraEleccionDelJuego = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        return primeraEleccionDelJuego;
    }

    private void SeleccionOpcionMostrarCartas()
    {
        EleccionesVerCartas eleccionesParaVerCartas = new EleccionesVerCartas(_jugadores, _numJugadorEnJuego, _view);
        eleccionesParaVerCartas.EleccionQueCartasVer();
    }
    private void SeleccionOpcionJugarCartas()
    {
        Superstar jugadorOponente = _jugadores[_numJugadorOponente];
        Superstar jugadorEnJuego = _jugadores[_numJugadorEnJuego];
        EleccionesJugarCarta eleccionDeCartasPorJugar = new EleccionesJugarCarta(jugadorEnJuego, jugadorOponente, _view);
        _continuarLoopElecciones = eleccionDeCartasPorJugar.ComenzarProcesoDeElecciones();
        if (!_continuarLoopElecciones)
        {
            _view.CongratulateWinner(_jugadores[_numJugadorEnJuego].Name);
            _continuarLoopPrincipal = _continuarLoopElecciones;
        }
    }

    private void SeleccionOpcionJugarHabilidad()
    {
        _jugadores[_numJugadorEnJuego].HabilidadEspecial(_view, _jugadores[_numJugadorOponente]);
    }

    private void SeleccionOpcionFinalizarTurno()
    {
        List<IViewableCardInfo> arsenalOponente = _jugadores[_numJugadorOponente].Arsenal;
        List<IViewableCardInfo> arsenalEnJuego = _jugadores[_numJugadorEnJuego].Arsenal;
        if (arsenalOponente.Count == 0)
            ElJugadorEnJuegoGana();
        
        else if (arsenalEnJuego.Count == 0)
            ElJugadorEnJuegoPierde();
        
        else
            TodaviaNoSeTerminaLaPartida();
    }

    private void ElJugadorEnJuegoGana()
    {
        _continuarLoopElecciones = false;
        _view.CongratulateWinner(_jugadores[_numJugadorEnJuego].Name);
        _continuarLoopPrincipal = _continuarLoopElecciones;
    }

    private void ElJugadorEnJuegoPierde()
    {
        _continuarLoopElecciones = false;
        _view.CongratulateWinner(_jugadores[_numJugadorOponente].Name);
        _continuarLoopPrincipal = _continuarLoopElecciones;
    }

    private void TodaviaNoSeTerminaLaPartida()
    {
        CambiarJugadores();
        RevisarJugadores();
        _continuarLoopElecciones = false;
    }

    private void SeleccionOpcionRendirse()
    {
        _view.CongratulateWinner(_jugadores[_numJugadorOponente].Name);
        _continuarLoopPrincipal = false;
        _continuarLoopElecciones = false;
    }

    private void ExtraerCartasInciales()
    {
        foreach (var jugador in _jugadores)
            jugador.SacarCartasAlInicio();
    }
    
    // 2 Abstraccion
    private void CambiarJugadores()
    {
        if (_numJugadorEnJuego == 0) { _numJugadorEnJuego = 1; }
        else { _numJugadorEnJuego = 0; }
    }
    private void RevisarJugadores()
    {
        if (_numJugadorEnJuego == 0) { _numJugadorOponente = 1; }
        else { _numJugadorOponente = 0; }
    }
    private string[] AperturaDeMazoSegunArchivo()
    {
        var mazoPath = _view.AskUserToSelectDeck(_deckFolder);
        var listaMazo = Utils.AbrirMazo(mazoPath);
        return listaMazo;
    }
    private int ValidarMazoParaContinuarJuego(Mazo mazo, int jugador)
    {
        if (!MazoValidator.ValidadorDeReglasDeMazo(mazo, _conjuntoCartas))
            jugador = MazoNoEsValido();
        else
            jugador = MazoEsValido(mazo, jugador);
        return jugador;
    }
    private void ElegirJugadorInicial()
    {
        if (!(_jugadores[0].SuperstarValue >= _jugadores[1].SuperstarValue))
        {
            Utils.CambiarPosicionesDeLaLista(_jugadores);
        }
    }
    
    // 3 Abstracción
    private int MazoEsValido(Mazo mazo, int jugador)
    {
        _jugadores.Add(mazo.SuperstarDelMazo);
        return jugador;
    }

    private int MazoNoEsValido()
    {
        _view.SayThatDeckIsInvalid();
        _continuarLoopPrincipal = false;
        int jugador = 2;
        return jugador;
    }
}