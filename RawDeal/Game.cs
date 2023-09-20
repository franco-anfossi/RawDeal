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
        string archivoCartas = Path.Combine("data", "cards.json");
        string archivoSuperstars = Path.Combine("data", "superstar.json");
        
        Utils.AbrirArchivo(archivoSuperstars);
        var superstarList = Utils.DeserializacionSuperstar();
        
        Utils.AbrirArchivo(archivoCartas);
        var cardsList = Utils.DeserializacionCartas();

        _conjuntoCartas = new ConjuntoCartas(cardsList, superstarList);
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        InicioEleccionMazo();
        if (_continuarLoopPrincipal)
        {
            ElegirJugadorInicial();
            ExtraerCartasInciales();
        }
        LoopInicialJuego();
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

    private void LoopInicialJuego()
    {
        while (_continuarLoopPrincipal && _numJugadorEnJuego < 2)
        {
            _continuarLoopElecciones = true;
            _view.SayThatATurnBegins(_jugadores[_numJugadorEnJuego].Name);

            _jugadores[_numJugadorEnJuego].SacarCarta();

            while (_continuarLoopElecciones)
            {
                _view.ShowGameInfo(_jugadores[_numJugadorEnJuego].DatosJugador, _jugadores[_numJugadorOponente].DatosJugador);
                

                var eleccionUno = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
                if (eleccionUno == NextPlay.ShowCards)
                {
                    EleccionesVerCartas.GetMazosUsuarios(_jugadores); 
                    EleccionesVerCartas.GetAtributoView(_view);
                    EleccionesVerCartas.GetNumJugadorEnJuego(_numJugadorEnJuego);
                    EleccionesVerCartas.EleccionQueCartasVer();
                }
                else if (eleccionUno == NextPlay.PlayCard)
                {
                    Superstar jugadorOponente = _jugadores[_numJugadorOponente];
                    Superstar jugadorEnJuego = _jugadores[_numJugadorEnJuego];
                    EleccionesJugarCarta.CrearJugadas(jugadorEnJuego, jugadorOponente);
                    EleccionesJugarCarta.FormatearJugada();
                    _continuarLoopElecciones = EleccionesJugarCarta.ComenzarProcesoDeElecciones(_view);
                    if (!_continuarLoopElecciones)
                    {
                        _view.CongratulateWinner(_jugadores[_numJugadorOponente].Name);
                        _continuarLoopPrincipal = _continuarLoopElecciones;
                    }
                }
                else if (eleccionUno == NextPlay.EndTurn)
                {
                    List<IViewableCardInfo> arsenalOponente = _jugadores[_numJugadorOponente].Arsenal;
                    if (arsenalOponente.Count == 0)
                    {
                        _continuarLoopElecciones = false;
                        _view.CongratulateWinner(_jugadores[_numJugadorEnJuego].Name);
                        _continuarLoopPrincipal = _continuarLoopElecciones;
                    }
                    else
                    {
                        CambiarJugadores();
                        RevisarJugadores();
                        _continuarLoopElecciones = false;
                    }
                    
                }
                else if (eleccionUno == NextPlay.GiveUp)
                {
                    _view.CongratulateWinner(_jugadores[_numJugadorOponente].Name);
                    _continuarLoopPrincipal = false;
                    _continuarLoopElecciones = false;
                }
            }
        }
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