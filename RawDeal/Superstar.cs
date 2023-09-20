using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public abstract class Superstar : IJugador
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
    public bool NoPuedeElegirUsarSuHabilidad = true;
    private int _fortitude;
    protected View _view;
    public abstract bool HabilidadEspecial(View view, Superstar oponente);

    public List<IViewableCardInfo> RevisarCartasJugables()
    {
        List<IViewableCardInfo> cartasJugables = new();
        foreach (var carta in Hand)
        {
            int intFortitude = Convert.ToInt32(carta.Fortitude);
            if (intFortitude <= _fortitude)
            {
                cartasJugables.Add(carta);
            }
        }

        return cartasJugables;
    }

    public override string ToString()
    {
        return $"{Name}";
    }

    public void InicializacionDeAtributos(List<IViewableCardInfo> mazo)
    {
        Arsenal = mazo;
        Hand = new List<IViewableCardInfo>();
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
        _fortitude = 0;
    }

    public void SacarCartasAlInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        ActualizarDatos();
    }

    public object Clonar()
    {
        return MemberwiseClone();
    }

    public void SacarCarta()
    {
        int ultimaCarta = Arsenal.Count - 1;
        if (ultimaCarta >= 0)
        {
            Hand.Add(Arsenal[ultimaCarta]);
            Arsenal.RemoveAt(ultimaCarta);
        }

        ActualizarDatos();
    }

    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(Name, _fortitude, Hand.Count, Arsenal.Count);
    }

    public void AgregarFortitudeSegunDano(int dano)
    {
        _fortitude += dano;
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

    public void PasarCartaDeRingsideAlArsenal(int indexCarta)
    {
        IViewableCardInfo cartaElegida = Ringside[indexCarta]; 
        Ringside.RemoveAt(indexCarta);
        Hand.Insert(0, cartaElegida);
        ActualizarDatos();
    }
    
    public void PasarCartaDeLaManoAlRingside(int indexCarta)
    {
        IViewableCardInfo cartaElegida = Hand[indexCarta]; 
        Hand.RemoveAt(indexCarta);
        Ringside.Add(cartaElegida);
        ActualizarDatos();
    }
}
    