using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public abstract class Superstar
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public int HandSize { get; set; }
    public int SuperstarValue { get; set; }
    public string SuperstarAbility { get; set; }
    public List<IViewableCardInfo> Arsenal { get; private set; }
    public PlayerInfo DatosJugador { get; private set; }
    public List<IViewableCardInfo> Hand { get; private set; }
    public List<IViewableCardInfo> Ringside { get; private set; }
    public List<IViewableCardInfo> RingArea { get; private set; }
    protected View View;
    protected Superstar Oponente;
    private int _fortitude;
    
    public abstract bool EjecutarHabilidadEspecial();
    
    public virtual bool ObtenerQueNoSePuedeEligirSiUsarLaHabilidad()
    {
        return true;
    }

    public virtual void CambiarVisibilidadDeElegirLaHabilidad()
    {
    }

    public void AgregarAtributosNecesarios(View view, Superstar oponente)
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
    