using RawDealView;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private ConjuntoCartas _conjuntoCartas;
    private List<Jugador> _jugadores = new();
    private bool _continuarLoop = true;
    
    public Game(View view, string deckFolder)
    {
        string archivoCartas = Path.Combine("data", "cards.json");
        string archivoSuperstars = Path.Combine("data", "superstar.json");

        var superstarList = Utils.AbrirArchivo<Superstar>(archivoSuperstars);
        var cardsList = Utils.AbrirArchivo<Carta>(archivoCartas);

        _conjuntoCartas = new ConjuntoCartas(cardsList, superstarList);
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        InicioEleccionMazo();
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
            ElegirJugadorInicial();
            _view.SayThatATurnBegins(_jugadores[0].MiSuperstar.Name);
            
            foreach (var jugador in _jugadores)
                jugador.SacarCartasInicio();
            
            _jugadores[0].SacarCarta();
        
            _view.ShowGameInfo(_jugadores[0].DatosJugador, _jugadores[1].DatosJugador);
            _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
            _view.CongratulateWinner(_jugadores[1].MiSuperstar.Name);
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
        if (!(_jugadores[0].MiSuperstar.SuperstarValue >= _jugadores[1].MiSuperstar.SuperstarValue))
        {
            Utils.CambiarPosicionesDeLaLista(_jugadores);
        }
    }
    
    // 3 Abstraccion
    private int MazoEsValido(Mazo mazo, int jugador)
    {
        mazo.AgregarSuperstarComoAtributo();
        _jugadores.Add(new Jugador(mazo));
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