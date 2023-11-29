using RawDealView.Formatters;

namespace RawDeal.Bonus.Conditions_Over_Play;

public abstract class ConditionOverPlay
{
    public abstract bool CheckIfBonusApplies(IViewablePlayInfo playInfo);
}