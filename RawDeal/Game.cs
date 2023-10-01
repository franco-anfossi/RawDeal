using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private ConjuntoCartas _conjuntoCartas;
    private List<Jugador> _jugadores = new();
    
    private bool _estadoLoopPrincipal = true;
    private bool _estadoLoopElecciones = true;
    
    private int _indiceJugadorEnJuego;
    private int _indiceJugadorOponente = 1;

    private Jugador _jugadorEnJuego;
    private Jugador _jugadorOponente;
    
    
    public Game(View view, string deckFolder)
    {
        var (superstarsDeserializados, cartasDeserializadas) = Utils.DeserializarDeCartasYSuperstarsDesdeLosJson();
        _conjuntoCartas = new ConjuntoCartas(cartasDeserializadas, superstarsDeserializados);
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        IniciarEleccionMazo();
        if (_estadoLoopPrincipal)
        {
            ElegirJugadorInicial();
            ExtraerCartasIniciales();
        }
        CorrerLoopPrincipalDelJuego();
    }
    private void IniciarEleccionMazo()
    {
        for (int indiceJugador = 0; indiceJugador < 2; indiceJugador++)
        {
            string[] mazoObtenidoDelArchivo = AbrirMazoSegunArchivo();
            Mazo nuevoMazo = new Mazo(mazoObtenidoDelArchivo, _conjuntoCartas);
            indiceJugador = ValidarMazoParaContinuarJuego(nuevoMazo, indiceJugador);
        }
    }
    private void ElegirJugadorInicial()
    {
        if (!(_jugadores[0].DarSuperstarValue() >= _jugadores[1].DarSuperstarValue()))
            Utils.CambiarPosicionesDeLaLista(_jugadores);
    }
    private void ExtraerCartasIniciales()
    {
        foreach (var jugador in _jugadores)
            jugador.SacarCartasAlInicio();
    }
    
    private void CorrerLoopPrincipalDelJuego()
    {
        while (_estadoLoopPrincipal)
        {
            _estadoLoopElecciones = true;
            InicializarVariablesDeJugadores();
            AgregarAtributosALosSuperstars();
            _jugadorEnJuego.DecirQueComienzaElTurno();
            EjecutarLaHabilidadEspecialAntesDeSacarCarta();
            _jugadorEnJuego.CambiarVisibilidadDeElegirLaHabilidad();
            CorrerLoopEleccionesDelJuego();
        }
    }
    
    private string[] AbrirMazoSegunArchivo()
    {
        string rutaDelMazo = _view.AskUserToSelectDeck(_deckFolder);
        var mazoAbiertoDeArchivo = Utils.AbrirMazo(rutaDelMazo);
        return mazoAbiertoDeArchivo;
    }
    private int ValidarMazoParaContinuarJuego(Mazo mazo, int indiceJugador)
    {
        if (!MazoValidator.ValidadorDeReglasDeMazo(mazo, _conjuntoCartas))
            indiceJugador = ManejarMazoNoValido();
        else
            indiceJugador = ManejarMazoValido(mazo, indiceJugador);
        return indiceJugador;
    }
    private void EjecutarLaHabilidadEspecialAntesDeSacarCarta()
    {
        bool estadoParaSacarCarta = true;

        if (_jugadorEnJuego.ObtenerQueNoSePuedeEligirSiUsarLaHabilidad())
            estadoParaSacarCarta = _jugadorEnJuego.EjecutarHabilidadEspecial();

        if (estadoParaSacarCarta)
            _jugadorEnJuego.SacarCarta();
    }
    private void CorrerLoopEleccionesDelJuego()
    {
        while (_estadoLoopElecciones)
        {
            _view.ShowGameInfo(_jugadorEnJuego.DarDatosJugador(), _jugadorOponente.DarDatosJugador());
            NextPlay eleccionDeLasPrimerasOpciones = EvaluarCondicionesParaMostrarLaEleccionDeHabilidad();
            ManejarEleccionesPosibles(eleccionDeLasPrimerasOpciones);
        }
    }

    private void InicializarVariablesDeJugadores()
    {
        _jugadorEnJuego = _jugadores[_indiceJugadorEnJuego];
        _jugadorOponente = _jugadores[_indiceJugadorOponente];
    }
    private void AgregarAtributosALosSuperstars()
    {
        _jugadorEnJuego.AgregarAtributosNecesarios(_view, _jugadorOponente);
        _jugadorOponente.AgregarAtributosNecesarios(_view, _jugadorEnJuego);
    }
    
    private NextPlay EvaluarCondicionesParaMostrarLaEleccionDeHabilidad()
    {
        NextPlay primerasOpcionesDeEleccionDelJuego;
        if (_jugadorEnJuego.ObtenerQueNoSePuedeEligirSiUsarLaHabilidad())
            primerasOpcionesDeEleccionDelJuego = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        else
            primerasOpcionesDeEleccionDelJuego = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible(); 
        return primerasOpcionesDeEleccionDelJuego;
    }
    
    private int ManejarMazoValido(Mazo mazo, int jugador)
    {
        _jugadores.Add(mazo.JugadorDelMazo);
        return jugador;
    }

    private int ManejarMazoNoValido()
    {
        _view.SayThatDeckIsInvalid();
        _estadoLoopPrincipal = false;
        int jugador = 2;
        return jugador;
    }

    private void ManejarEleccionesPosibles(NextPlay eleccionDeLaPrimeraOpcion)
    {
        if (eleccionDeLaPrimeraOpcion == NextPlay.ShowCards)
            SeleccionarOpcionMostrarCartas();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.PlayCard)
            SeleccionarOpcionJugarCartas();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.UseAbility)
            SeleccionarOpcionJugarHabilidad();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.EndTurn)
            SeleccionarOpcionFinalizarTurno();
            
        else if (eleccionDeLaPrimeraOpcion == NextPlay.GiveUp)
            SeleccionarOpcionRendirse();
    }
    
    private void SeleccionarOpcionMostrarCartas()
    {
        EleccionesVerCartas eleccionesParaVerCartas = new EleccionesVerCartas(_jugadores, _indiceJugadorEnJuego, _view);
        eleccionesParaVerCartas.ElegirQueCartasVer();
    }
    private void SeleccionarOpcionJugarCartas()
    {
        EleccionesJugarCarta eleccionDeCartasPorJugar = new EleccionesJugarCarta(_jugadorEnJuego, _jugadorOponente, _view);
        _estadoLoopElecciones = eleccionDeCartasPorJugar.ComenzarProcesoDeElecciones();
        if (!_estadoLoopElecciones)
            DeclararVictoriaDelJugadorEnJuego();
    }

    private void SeleccionarOpcionJugarHabilidad()
    {
        _jugadorEnJuego.EjecutarHabilidadEspecial();
    }

    private void SeleccionarOpcionFinalizarTurno()
    {
        if (_jugadorOponente.ComprobarArsenalVacio())
            DeclararVictoriaDelJugadorEnJuego();
        
        else if (_jugadorEnJuego.ComprobarArsenalVacio())
            DeclararDerrotaDelJugadorEnJuego();
        
        else
            CambiarLaPosicionDeLosJugadores();
    }

    private void SeleccionarOpcionRendirse()
    {
        _jugadorOponente.AvisarGanador();
        _estadoLoopPrincipal = false; 
        _estadoLoopElecciones = false;
    }

    private void DeclararVictoriaDelJugadorEnJuego()
    {
        _estadoLoopElecciones = false;
        _jugadorEnJuego.AvisarGanador();
        _estadoLoopPrincipal = _estadoLoopElecciones;
    }

    private void DeclararDerrotaDelJugadorEnJuego()
    {
        _estadoLoopElecciones = false;
        _jugadorOponente.AvisarGanador();
        _estadoLoopPrincipal = _estadoLoopElecciones;
    }

    private void CambiarLaPosicionDeLosJugadores()
    {
        CambiarJugadores();
        RevisarJugadores();
        _estadoLoopElecciones = false;
    }
    
    private void CambiarJugadores()
    {
        if (_indiceJugadorEnJuego == 0)
        {
            _indiceJugadorEnJuego = 1;
        }
        else
        {
            _indiceJugadorEnJuego = 0;
        }

        _jugadorEnJuego = _jugadores[_indiceJugadorEnJuego];
    }
    private void RevisarJugadores()
    {
        if (_indiceJugadorEnJuego == 0)
        {
            _indiceJugadorOponente = 1;
        }
        else
        {
            _indiceJugadorOponente = 0;
        }

        _jugadorOponente = _jugadores[_indiceJugadorOponente];
    }
}