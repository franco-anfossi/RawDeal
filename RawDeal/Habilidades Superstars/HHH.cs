using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class HHH : Jugador
{
    public HHH(Superstar superstar) : base(superstar)
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
}