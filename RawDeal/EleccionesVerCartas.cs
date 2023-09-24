using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

namespace RawDeal;

public class EleccionesVerCartas
{
    private List<Superstar> _jugadores;
    private int _indiceJugadorEnJuego;
    private int _indiceJugadorOponente;
    private View _view;
    
    private Dictionary<int, List<List<IViewableCardInfo>>> _mazosBrutosDeLosJugadores = new();
    private Dictionary<int, List<List<string>>> _mazosFormateadosDeLosJugadores = new();

    private List<string> _manoJugadorEnJuego;
    private List<string> _ringsideJugadorEnJuego;
    private List<string> _ringAreaJugadorEnJuego;
    
    private List<string> _ringsideJugadorOponente;
    private List<string> _ringAreaJugadorOponente;

    public EleccionesVerCartas(List<Superstar> jugadores, int numIndiceJugador, View view)
    {
        _jugadores = jugadores;
        _indiceJugadorEnJuego = numIndiceJugador;
        _view = view;
        
        ObtenerMazosUsuarios();
        FormatearMazosDeCartas();
        
        ArreglarNumJugadores();
        AgregarCartasAAtributos();
    }
    
    private void ObtenerMazosUsuarios()
    {
        for (int indiceJugador = 0; indiceJugador < 2; indiceJugador++)
        {
            List<List<IViewableCardInfo>> mazosDeUnJugador = ObtenerMazosDeUnJugador(indiceJugador);
            _mazosBrutosDeLosJugadores[indiceJugador] = mazosDeUnJugador;
        }
    }
    private void ArreglarNumJugadores()
    {
        if (_indiceJugadorEnJuego == 0) { _indiceJugadorOponente = 1; }
        else { _indiceJugadorOponente = 0; }
    }
    
    public void ElegirQueCartasVer()
    {
        var eleccionQueMazoVer = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (eleccionQueMazoVer == CardSet.Hand)
            VerMano();
        else if (eleccionQueMazoVer == CardSet.RingsidePile)
            VerMiRingside();
        else if (eleccionQueMazoVer == CardSet.RingArea)
            VerMiRingArea();
        else if (eleccionQueMazoVer == CardSet.OpponentsRingsidePile)
            VerRingsideOponente();
        else if (eleccionQueMazoVer == CardSet.OpponentsRingArea)
            VerRingAreaOponente();
    }
    private void AgregarCartasAAtributos()
    { 
        _manoJugadorEnJuego = _mazosFormateadosDeLosJugadores[_indiceJugadorEnJuego][0];
        _ringsideJugadorEnJuego = _mazosFormateadosDeLosJugadores[_indiceJugadorEnJuego][1];
        _ringAreaJugadorEnJuego = _mazosFormateadosDeLosJugadores[_indiceJugadorEnJuego][2];
        
        _ringsideJugadorOponente = _mazosFormateadosDeLosJugadores[_indiceJugadorOponente][1];
        _ringAreaJugadorOponente = _mazosFormateadosDeLosJugadores[_indiceJugadorOponente][2];
    }
    
    private void FormatearMazosDeCartas()
    {
        int numeroParaLlaveDelDiccionario = 0;
        foreach (var mazosCartasUnJugador in _mazosBrutosDeLosJugadores)
        {
            List<List<string>> mazoCartasFormateadas = FormatearMazoEspecificoGuardadoEnDiccionario(mazosCartasUnJugador.Value);
            _mazosFormateadosDeLosJugadores[numeroParaLlaveDelDiccionario] = mazoCartasFormateadas;
            numeroParaLlaveDelDiccionario++;
        }
    }
    
    private List<List<string>> FormatearMazoEspecificoGuardadoEnDiccionario(List<List<IViewableCardInfo>> mazosCartasUnJugador)
    {
        List<List<string>> mazoCartasFormateadas = new List<List<string>>();
        foreach (var mazoCartas in mazosCartasUnJugador)
        {
            List<string> datosFormateadosDeLasCartas = Utils.FormatearMazoDeCartas(mazoCartas);
            mazoCartasFormateadas.Add(datosFormateadosDeLasCartas);
        }

        return mazoCartasFormateadas;
    }
    private List<List<IViewableCardInfo>> ObtenerMazosDeUnJugador(int indiceJugador)
    {
        List<List<IViewableCardInfo>> mazosUnJugador = new List<List<IViewableCardInfo>>();
        
        List<IViewableCardInfo> manoJugador = _jugadores[indiceJugador].Hand;
        List<IViewableCardInfo> ringsideJugador = _jugadores[indiceJugador].Ringside;
        List<IViewableCardInfo> ringAreaJugador = _jugadores[indiceJugador].RingArea;
        
        mazosUnJugador.Add(manoJugador);
        mazosUnJugador.Add(ringsideJugador);
        mazosUnJugador.Add(ringAreaJugador);
        
        return mazosUnJugador;
    }
    
    private void VerMano() => _view.ShowCards(_manoJugadorEnJuego); 
    private void VerMiRingside() => _view.ShowCards(_ringsideJugadorEnJuego);
    private void VerMiRingArea() => _view.ShowCards(_ringAreaJugadorEnJuego);
    private void VerRingsideOponente() => _view.ShowCards(_ringsideJugadorOponente);
    private void VerRingAreaOponente() => _view.ShowCards(_ringAreaJugadorOponente);
}