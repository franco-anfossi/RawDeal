using RawDealView;

namespace RawDeal;

public abstract class Superstar : IJugador
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public int HandSize { get; set; }
    public int SuperstarValue { get; set; }
    public string SuperstarAbility { get; set; }

    public List<Carta> Arsenal { get; private set; }
    public PlayerInfo DatosJugador { get; private set; }

    public List<Carta> Hand { get; private set; }
    public List<Carta> Ringside { get; private set; }
    public List<Carta> RingArea { get; private set; }
    private int _fortitude;
    public abstract void HabilidadEspecial();

    public override string ToString()
    {
        return $"{Name}";
    }

    public void InicializacionDeAtributos(List<Carta> mazo)
    {
        Arsenal = mazo;
        Hand = new List<Carta>();
        Ringside = new List<Carta>();
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
}
    