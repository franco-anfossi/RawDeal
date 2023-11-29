using RawDeal.Bonus.Conditions_Over_Play;
using RawDeal.Bonus.Expiration_Conditions;
using RawDealView.Formatters;

namespace RawDeal.Bonus.Bonuses;

public abstract class Bonus
{
    private readonly ExpirationCondition _expirationCondition;
    private readonly ConditionOverPlay _applicationCondition;
    
    public void ApplyBonus()
    {
    }
    
    public void CheckIfBonusIsExpired()
    {
    }
    
    private void CheckIfBonusApplies(IViewablePlayInfo playInfo)
    {
    }
    
    public abstract void AddBonusTo(IViewablePlayInfo playInfo);
}