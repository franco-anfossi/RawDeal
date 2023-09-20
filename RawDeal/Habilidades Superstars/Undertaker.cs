using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Undertaker : Superstar
{
    public bool NoPuedeElegirUsarSuHabilidad = false;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        // int indexCartaElegida = _view.AskPlayerToSelectACardToDiscard()
        return false;
    }
}