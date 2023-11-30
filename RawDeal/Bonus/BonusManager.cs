using RawDeal.Boundaries;
using RawDealView.Formatters;

namespace RawDeal.Bonus;

public class BonusManager
{
    private readonly BoundaryList<Bonus> _activeBonuses = new();
    
    public void AddBonus(Bonus bonus)
    {
        _activeBonuses.Add(bonus);
    }
    
    public void RemoveExpiredBonuses()
    {
        _activeBonuses.RemoveAll(bonus => bonus.CheckIfBonusIsExpired());
    }
    
    public void ApplyBonuses(IViewablePlayInfo playInfo)
    {
        foreach (var bonus in _activeBonuses)
            bonus.ApplyBonus(playInfo);
    }
}