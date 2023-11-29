using RawDeal.Boundaries;
using RawDealView.Formatters;

namespace RawDeal.Bonus;

public class BonusManager
{
    private readonly BoundaryList<Bonuses.Bonus> _activeBonuses;
    
    public BonusManager()
    {
        _activeBonuses = new BoundaryList<Bonuses.Bonus>();
    }
    
    public void AddBonus(Bonuses.Bonus bonus)
    {
        _activeBonuses.Add(bonus);
    }
    
    public void RemoveExpiredBonuses()
    {
    }
    
    public void ApplyBonuses(IViewablePlayInfo playInfo)
    {
    }
}