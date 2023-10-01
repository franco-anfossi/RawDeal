using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public abstract class Jugador
{
    protected View View;
    protected Jugador Oponente;
    
    protected string Name;
    protected string Logo;
    protected int HandSize;
    protected int SuperstarValue;
    protected string SuperstarAbility;
    protected PlayerInfo DatosJugador;
    protected List<IViewableCardInfo> Hand;
    protected List<IViewableCardInfo> Arsenal;
    protected List<IViewableCardInfo> Ringside;
    protected List<IViewableCardInfo> RingArea;
    protected Dictionary<string, List<IViewableCardInfo>> _mazos;
    private int _fortitude;

    protected Jugador(Superstar superstar)
    {
        Name = superstar.Name;
        Logo = superstar.Logo;
        HandSize = superstar.HandSize;
        SuperstarValue = superstar.SuperstarValue;
        SuperstarAbility = superstar.SuperstarAbility;
    }

    public abstract bool EjecutarHabilidadEspecial();
    
    public virtual bool ObtenerQueNoSePuedeEligirSiUsarLaHabilidad()
    {
        return true;
    }

    public virtual void CambiarVisibilidadDeElegirLaHabilidad()
    {
    }
    public void JugadorRecibiraDano(int danoRecibido)
    {
        View.SayThatSuperstarWillTakeSomeDamage(Name, danoRecibido);
    }

    public int PreguntarPorCartaPorDescartar(List<string> cartasFormateadas, int cantidadCartas)
    {
        return View.AskPlayerToSelectACardToDiscard(cartasFormateadas, Name, Name, cantidadCartas);
    }

    public string ObtenerLogo()
    {
        return Logo;
    }

    public List<IViewableCardInfo> ObtenerMano()
    {
        return Hand;
    }

    public void AgregarAtributosNecesarios(View view, Jugador oponente)
    {
        View = view;
        Oponente = oponente;
    }

    public bool ComprobarArsenalVacio()
    {
        if (Arsenal.Count != 0) { return false; }
        return true;
    }
    public List<IViewableCardInfo> RevisarCartasJugables()
    {
        List<IViewableCardInfo> cartasJugables = new();
        foreach (var carta in Hand)
        {
            int intFortitude = Convert.ToInt32(carta.Fortitude);
            if (intFortitude <= _fortitude) { cartasJugables.Add(carta); }
        }

        return cartasJugables;
    }

    public void InicializarLosAtributosNecesarios(List<IViewableCardInfo> mazo)
    {
        Arsenal = mazo;
        Hand = new List<IViewableCardInfo>();
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
        _fortitude = 0;
    }
    public List<IViewableCardInfo> getHand()
    { 
        return Hand;
    }
    
    public List<IViewableCardInfo> getRingside()
    { 
        return Ringside;
    }
    
    public List<IViewableCardInfo> getRingArea()
    { 
        return RingArea;
    }

    public virtual void SacarCartasAlInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        ActualizarDatos();
    }

    public object Clonar()
    {
        return MemberwiseClone();
    }
    
    public bool CompararNombres(string nombre)
    {
        return Name == nombre;
    }

    public void AvisarGanador()
    {
        View.CongratulateWinner(Name);
    }
    
    public void DecirQueComienzaElTurno()
    {
        View.SayThatATurnBegins(Name);
    }

    public PlayerInfo DarDatosJugador()
    {
        return DatosJugador;
    }

    public int DarSuperstarValue()
    {
        return SuperstarValue;
    }

    public void IntentarJugarCarta(string cartaElegida)
    {
        View.SayThatPlayerIsTryingToPlayThisCard(Name, cartaElegida);
    }

    public bool RevisarSiEsManking()
    {
        return Name == "MANKIND";
    }

    public Dictionary<string, List<IViewableCardInfo>> ObtenerMazos()
    {
        return _mazos;
    } 
    
    public void HacerDanoAlOponente(int danoHecho)
    {
        int fortitudPorAgregar = danoHecho;
        if (Oponente.Name == "MANKIND")
            danoHecho--;
        
        View.SayThatSuperstarWillTakeSomeDamage(Oponente.Name, danoHecho);
        AgregarFortitudeSegunDano(fortitudPorAgregar);
    }

    public void PasarCartaDesdeUnMazoHastaFondoArsenal(List<IViewableCardInfo> mazoDesde, int indiceCarta)
    {
        IViewableCardInfo cartaElegida = mazoDesde[indiceCarta]; 
        mazoDesde.RemoveAt(indiceCarta);
        Arsenal.Insert(0, cartaElegida);
        ActualizarDatos();
    }
    public IViewableCardInfo PasarCartaDeArsenalARingside()
    {
        int largoArsenal = Arsenal.Count;
        IViewableCardInfo cartaExtraida = Arsenal[largoArsenal - 1];
        Arsenal.RemoveAt(largoArsenal - 1);
        Ringside.Add(cartaExtraida);
        ActualizarDatos();
        return cartaExtraida;
    }
    
    public void PasarCartaDeManoARingArea(IViewableCardInfo cartaSeleccionada)
    {
        Hand.Remove(cartaSeleccionada);
        RingArea.Add(cartaSeleccionada);
        ActualizarDatos();
    }
    
    public void PasarCartaDeLaManoAlRingside(int indiceCarta)
    {
        IViewableCardInfo cartaElegida = Hand[indiceCarta]; 
        Ringside.Add(cartaElegida);
        Hand.RemoveAt(indiceCarta);
        ActualizarDatos();
    }
    
    public void PasarCartaDelRingsideALaMano(int indiceCarta)
    {
        IViewableCardInfo cartaElegida = Ringside[indiceCarta]; 
        Ringside.RemoveAt(indiceCarta);
        Hand.Add(cartaElegida);
        ActualizarDatos();
    }

    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(Name, _fortitude, Hand.Count, Arsenal.Count);
    }
    private void AgregarFortitudeSegunDano(int dano)
    {
        _fortitude += dano;
        ActualizarDatos();
    }
    public virtual void SacarCarta()
    {
        int ultimaCartaDeArsenal = Arsenal.Count - 1;
        if (ultimaCartaDeArsenal >= 0)
        {
            Hand.Add(Arsenal[ultimaCartaDeArsenal]);
            Arsenal.RemoveAt(ultimaCartaDeArsenal);
        }

        ActualizarDatos();
    }
}
    