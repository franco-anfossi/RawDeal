using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

namespace RawDeal;

public static class EleccionesVerCartas
{
    private static int _jugadorEnJuego;
    private static int _jugadorOponente;
    private static View _view;
    private static Dictionary<int, List<List<Carta>>> _mazosBrutosJugadores = new();
    private static Dictionary<int, List<List<string>>> _mazosFormateadosJugadores = new();
    
    

    public static void GetMazosUsuarios(List<Superstar> jugadores)
    {
        for (int numJugador = 0; numJugador < jugadores.Count; numJugador++)
        {
            List<Carta> manoJugador = jugadores[numJugador].Hand;
            List<Carta> ringsideJugador = jugadores[numJugador].Ringside;
            List<Carta> ringAreaJugador = jugadores[numJugador].RingArea;

            List<List<Carta>> mazosUnJugador = new List<List<Carta>>();
            mazosUnJugador.Add(manoJugador);
            mazosUnJugador.Add(ringsideJugador);
            mazosUnJugador.Add(ringAreaJugador);
            _mazosBrutosJugadores[numJugador] = mazosUnJugador;
        }
        FormatearCartas();
    }
    
    public static void FormatearCartas()
    {
        int numAuxliar = 0;
        foreach (var mazosCartasUnJugador in _mazosBrutosJugadores)
        {
            List<List<string>> mazoCartasFormateadas = new List<List<string>>();
            foreach (var mazoCartas in mazosCartasUnJugador.Value)
            {
                List<string> datosDeLasCartas = new List<string>();
                foreach (var carta in mazoCartas)
                {
                    string cartaFormateada = Formatter.CardToString(carta);
                    datosDeLasCartas.Add(cartaFormateada);
                }
                mazoCartasFormateadas.Add(datosDeLasCartas);
            }
            _mazosFormateadosJugadores[numAuxliar] = mazoCartasFormateadas;
            numAuxliar++;
        }
    }

    public static void GetNumJugadorEnJuego(int numJugador)
    {
        _jugadorEnJuego = numJugador;
        if (_jugadorEnJuego == 0) { _jugadorOponente = 1; }
        else { _jugadorOponente = 0; }
    }

    public static void GetAtributoView(View view)
    {
        _view = view;
    }

    public static void EleccionQueCartasVer()
    {
        var eleccionDos = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (eleccionDos == CardSet.Hand)
            VerMano();
        else if (eleccionDos == CardSet.RingsidePile)
            VerMiRingside();
        else if (eleccionDos == CardSet.RingArea)
            VerMiRingArea();
        else if (eleccionDos == CardSet.OpponentsRingsidePile)
            VerRingsideOponente();
        else if (eleccionDos == CardSet.OpponentsRingArea)
            VerRingAreaOponente();
    }

    private static void VerMano()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][0]); 
    }

    private static void VerMiRingside()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][1]);
    }

    private static void VerMiRingArea()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][2]);
    }

    private static void VerRingsideOponente()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorOponente][1]);
    }

    private static void VerRingAreaOponente()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorOponente][2]);
    }
}