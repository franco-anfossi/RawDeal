using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Kane : Superstar
{
    public override bool EjecutarHabilidadEspecial()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        View.SayThatSuperstarWillTakeSomeDamage(Oponente.Name, 1);
        
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