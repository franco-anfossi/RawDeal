using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Kane : Superstar
{
    public new bool NoPuedeElegirUsarSuHabilidad = true;
    
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        bool continuarPartida = true;
        _view = view;
        _view.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        _view.SayThatSuperstarWillTakeSomeDamage(oponente.Name, 1);
        for (int iteracionDelDano = 1; iteracionDelDano <= 1; iteracionDelDano++)
        {
            if (oponente.Arsenal.Count != 0)
            {
                IViewableCardInfo cartaExtraida = oponente.PasarCartaDeArsenalARingside();
                string cartaExtraidaFormateada = Formatter.CardToString(cartaExtraida);

                _view.ShowCardOverturnByTakingDamage(cartaExtraidaFormateada, iteracionDelDano, 1);
            }
            else
                continuarPartida = false;
        }

        return true;
    }
}