using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

namespace RawDeal;

public class EleccionesVerCartas
{
    private int _jugadorEnJuego;
    private int _jugadorOponente;
    private View _view;
    private Dictionary<int, List<List<IViewableCardInfo>>> _mazosBrutosJugadores = new();
    private Dictionary<int, List<List<string>>> _mazosFormateadosJugadores = new();

    public EleccionesVerCartas(List<Superstar> jugadores, int numJugador, View view)
    {
        _jugadorEnJuego = numJugador;
        _view = view;
        GetMazosUsuarios(jugadores);
        ArreglarNumJugadores();
    }
    

    public void GetMazosUsuarios(List<Superstar> jugadores)
    {
        for (int numJugador = 0; numJugador < jugadores.Count; numJugador++)
        {
            List<IViewableCardInfo> manoJugador = jugadores[numJugador].Hand;
            List<IViewableCardInfo> ringsideJugador = jugadores[numJugador].Ringside;
            List<IViewableCardInfo> ringAreaJugador = jugadores[numJugador].RingArea;

            List<List<IViewableCardInfo>> mazosUnJugador = new List<List<IViewableCardInfo>>();
            mazosUnJugador.Add(manoJugador);
            mazosUnJugador.Add(ringsideJugador);
            mazosUnJugador.Add(ringAreaJugador);
            _mazosBrutosJugadores[numJugador] = mazosUnJugador;
        }
        FormatearCartas();
    }
    
    public void FormatearCartas()
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

    public void ArreglarNumJugadores()
    {
        if (_jugadorEnJuego == 0) { _jugadorOponente = 1; }
        else { _jugadorOponente = 0; }
    }

    public void EleccionQueCartasVer()
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

    private void VerMano()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][0]); 
    }

    private void VerMiRingside()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][1]);
    }

    private void VerMiRingArea()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorEnJuego][2]);
    }

    private void VerRingsideOponente()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorOponente][1]);
    }

    private void VerRingAreaOponente()
    {
        _view.ShowCards(_mazosFormateadosJugadores[_jugadorOponente][2]);
    }
}