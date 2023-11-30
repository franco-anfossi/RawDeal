using RawDeal.Bonus.Conditions_Over_Play;
using RawDeal.Bonus.Expiration_Conditions;
using RawDealView.Formatters;

namespace RawDeal.Bonus;

public abstract class Bonus
{
    private readonly ExpirationCondition _expirationCondition;
    private readonly ConditionOverPlay _applicationCondition;
    
    public void ApplyBonus(IViewablePlayInfo playInfo)
    {
        if (CheckIfBonusApplies(playInfo))
            AddBonusTo(playInfo);
    }
    
    public bool CheckIfBonusIsExpired()
    {
        return _expirationCondition.CheckIfBonusIsExpired();
    }
    
    private bool CheckIfBonusApplies(IViewablePlayInfo playInfo)
    {
        return _applicationCondition.CheckIfBonusApplies(playInfo);
    }
    
    public abstract void AddBonusTo(IViewablePlayInfo playInfo);
}