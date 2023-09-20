using RawDealView;

namespace RawDeal.Habilidades_Superstars;

public class HHH : Superstar
{
    public new bool NoPuedeElegirUsarSuHabilidad = true;
    
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        return true;
    }
}