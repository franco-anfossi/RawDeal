using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Kane : Jugador
{
    public Kane(Superstar superstar) : base(superstar)
    {
        Name = superstar.Name;
        Logo = superstar.Logo;
        HandSize = superstar.HandSize;
        SuperstarValue = superstar.SuperstarValue;
        SuperstarAbility = superstar.SuperstarAbility;
    }
    
    public override bool EjecutarHabilidadEspecial()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        Oponente.JugadorRecibiraDano(1);
        
        if (!Oponente.ComprobarArsenalVacio())
            RetirarUnaCartaDelOponenteAlRingside();

        return true;
    }

    private void RetirarUnaCartaDelOponenteAlRingside()
    {
        IViewableCardInfo cartaExtraida = Oponente.PasarCartaDeArsenalARingside();
        string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);
        View.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, 1, 1);
    }
}