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
    
    private List<Carta> _hand = new();
    private List<Carta> _ringside = new();
    private int _fortitude = 0;
    public abstract void HabilidadEspecial();
    public override string ToString()
    {
        return $"{Name}";
    }

    public void IngresarMazo(List<Carta> mazo)
    {
        Arsenal = mazo;
    }

    public void SacarCartasAlInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        ActualizarDatos();
    }

    public void SacarCarta()
    {
        int ultimaCarta = Arsenal.Count - 1;  
        if (ultimaCarta >= 0)
        {
            _hand.Add(Arsenal[ultimaCarta]);
            Arsenal.RemoveAt(ultimaCarta);
        }
        ActualizarDatos();
    }

    public void ActualizarDatos()
    {
        DatosJugador = new PlayerInfo(Name, _fortitude, _hand.Count, Arsenal.Count);
    }
}