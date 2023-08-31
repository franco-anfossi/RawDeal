using RawDealView;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private ConjuntoCartas _conjuntoCartas;
    private List<Jugador> _jugadores = new();
    
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
        List<Jugador> jugadores = new List<Jugador>();
        bool continuar = true;
        for (int i = 0; i < 2; i++)
        {
            var mazoPath = _view.AskUserToSelectDeck(_deckFolder);
            var listaMazo = Utils.AbrirMazo(mazoPath);
            Mazo mazo = new Mazo(listaMazo, _conjuntoCartas);
            if (!MazoValidator.Validate(mazo, _conjuntoCartas))
            {
                _view.SayThatDeckIsInvalid();
                i = 2;
                continuar = false;
            }
            else
            {
                mazo.AgregarSuperstar();
                jugadores.Add(new Jugador(mazo));
            }
        }

        while (continuar)
        {
            JugadorInicial(jugadores);
            _view.SayThatATurnBegins(_jugadores[0].MiSuperstar.Name);
            
            foreach (var jugador in _jugadores)
                jugador.SacarCartasInicio();
            
            _jugadores[0].SacarCarta();
        
            _view.ShowGameInfo(_jugadores[0].DatosJugador, _jugadores[1].DatosJugador);
            _view.AskUserWhatToDoWhenItIsNotPossibleToUseItsAbility();
            _view.CongratulateWinner(_jugadores[1].MiSuperstar.Name);
            continuar = false;
        }
    }

    private void JugadorInicial(List<Jugador> jugadores)
    {
        if (jugadores[0].MiSuperstar.SuperstarValue >= jugadores[1].MiSuperstar.SuperstarValue)
        {
            _jugadores.Add(jugadores[0]);
            _jugadores.Add(jugadores[1]);
        }
        else if (jugadores[0].MiSuperstar.SuperstarValue < jugadores[1].MiSuperstar.SuperstarValue)
        {
            _jugadores.Add(jugadores[1]);
            _jugadores.Add(jugadores[0]);
        }
        /*else
        {
            Random rnd = new Random();
            int indice = rnd.Next(2);
            _jugadores.Add(jugadores[indice]);
            jugadores.RemoveAt(indice);
            _jugadores.Add(jugadores[0]);
        }*/
    }
}