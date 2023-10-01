using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Mankind : Jugador
{
    private bool _sacadaDeCartasInicial = true;
    private int _ultimaCartaDelArsenal;
    
    public Mankind(Superstar superstar) : base(superstar)
    {
        Name = superstar.Name;
        Logo = superstar.Logo;
        HandSize = superstar.HandSize;
        SuperstarValue = superstar.SuperstarValue;
        SuperstarAbility = superstar.SuperstarAbility;
    }
    
    public override bool EjecutarHabilidadEspecial()
    {
        return true;
    }
    public override void SacarCartasAlInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        
        _sacadaDeCartasInicial = false;
        ActualizarDatos();
    }
    public override void SacarCarta()
    {
        if (Arsenal == null)
        {
            throw new InvalidOperationException("El arsenal no ha sido inicializado.");
        }
        _ultimaCartaDelArsenal = Arsenal.Count - 1;
        if (VerificarCondicionParaRobarDosCartas())
            RobarDosCartas();
        else
        {
            Hand.Add(Arsenal[_ultimaCartaDelArsenal]); 
            Arsenal.RemoveAt(_ultimaCartaDelArsenal);
        }

        ActualizarDatos();
    }
    
    private bool VerificarCondicionParaRobarDosCartas()
    {
        return _ultimaCartaDelArsenal >= 1 && Arsenal.Count >= 1 && !_sacadaDeCartasInicial;
    }

    private void RobarDosCartas()
    {
        for (int restaPosicionCarta = 0; restaPosicionCarta < 2; restaPosicionCarta++)
        { 
            Hand.Add(Arsenal[_ultimaCartaDelArsenal - restaPosicionCarta]); 
            Arsenal.RemoveAt(_ultimaCartaDelArsenal - restaPosicionCarta); 
        }
    }
    
    
}