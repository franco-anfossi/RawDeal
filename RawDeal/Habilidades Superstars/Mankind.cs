using RawDealView;

namespace RawDeal.Habilidades_Superstars;

public class Mankind : Superstar
{
    private bool _sacadaInicial = true;
    public override void SacarCartasAlInicio()
    {
        for (int i = 0; i < HandSize; i++)
            SacarCarta();
        _sacadaInicial = false;
        ActualizarDatos();
    }
    public override void SacarCarta()
    {
        int ultimaCarta = Arsenal.Count - 1;
        int penultimaCarta = Arsenal.Count - 2;
        if (ultimaCarta >= 1 && Arsenal.Count >= 1 && penultimaCarta >= 0 && !_sacadaInicial)
        {
            for (int restaPosicionCarta = 0; restaPosicionCarta < 2; restaPosicionCarta++)
            { 
                Hand.Add(Arsenal[ultimaCarta - restaPosicionCarta]); 
                Arsenal.RemoveAt(ultimaCarta - restaPosicionCarta); 
            }
        }
        else
        {
            Hand.Add(Arsenal[ultimaCarta]); 
            Arsenal.RemoveAt(ultimaCarta);
        }

        ActualizarDatos();
    }
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        return true;
    }
}