namespace RawDeal.Habilidades_Superstars;

public class Mankind : Superstar
{
    private bool _sacadaDeCartasInicial = true;
    private int _ultimaCartaDelArsenal;
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
        if (_ultimaCartaDelArsenal >= 1 && Arsenal.Count >= 1 && !_sacadaDeCartasInicial) { return true; }
        return false;
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