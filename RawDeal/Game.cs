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
    private bool _continuarLoop = true;
    
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
        if (_continuarLoop)
            ElegirJugadorInicial();
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
        while (_continuarLoop)
        {
            _view.SayThatATurnBegins(_jugadores[0].Name);
            
            foreach (var jugador in _jugadores)
            {
                jugador.SacarCartasAlInicio();
            }
            
            _jugadores[0].SacarCarta();
            
            _view.ShowGameInfo(_jugadores[0].DatosJugador, _jugadores[1].DatosJugador);
            var eleccionUno = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
            if (eleccionUno == NextPlay.ShowCards)
            {
                var eleccionDos = _view.AskUserWhatSetOfCardsHeWantsToSee();
                if (eleccionDos == CardSet.Hand)
                {
                    List<string> datosDeLasCartas = new List<string>();
                    foreach (var carta in _jugadores[0].Hand)
                    {
                        string cartaFormateada = Formatter.CardToString(carta);
                        datosDeLasCartas.Add(cartaFormateada);
                    }
                    
                    _view.ShowCards(datosDeLasCartas);  
                }
            }
            
            _view.CongratulateWinner(_jugadores[1].Name);
            _continuarLoop = false;
        }
    }
    
    // 2 Abstraccion
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
        _continuarLoop = false;
        int jugador = 2;
        return jugador;
    }
}