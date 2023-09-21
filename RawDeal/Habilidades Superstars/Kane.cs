using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Kane : Superstar
{
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        bool continuarPartida = true;
        View = view;
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        View.SayThatSuperstarWillTakeSomeDamage(oponente.Name, 1);
        for (int iteracionDelDano = 1; iteracionDelDano <= 1; iteracionDelDano++)
        {
            if (oponente.Arsenal.Count != 0)
            {
                IViewableCardInfo cartaExtraida = oponente.PasarCartaDeArsenalARingside();
                string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);

                View.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, iteracionDelDano, 1);
            }
            else
                continuarPartida = false;
        }

        return true;
    }
}