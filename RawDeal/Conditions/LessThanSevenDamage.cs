using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class LessThanSevenDamage : Condition
{
    private readonly ImportantPlayerData _playerData;
    
    public LessThanSevenDamage(ImportantPlayerData playerData, IViewablePlayInfo selectedPlay) : base(selectedPlay)
        => _playerData = playerData;

    public override bool Check()
    {
        int damageChanges = HandleDamageAddedByEffects() - HandleDamageReducedByEffects();
        var cardDamage = Convert.ToInt32(SelectedPlay.CardInfo.Damage) + damageChanges;
        return cardDamage <= 7;
    }
    
    private int HandleDamageReducedByEffects()
    {
        int damageReduced = _playerData.BonusSet.MankindBonusDamageChange.MankindOpponentDamageChange;
        return damageReduced;
    }
    
    private int HandleDamageAddedByEffects()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded += _playerData.BonusSet.ChangesByJockeyingForPosition.DamageAdded;
        
        if (CheckIfSelectedCardIsStrike())
            damageAdded += _playerData.BonusSet.ChangesByIrishWhip.DamageAdded;
        
        return damageAdded;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
        => SelectedPlay.CardInfo.Subtypes.Contains("Grapple");
    
    private bool CheckIfSelectedCardIsStrike()
        => SelectedPlay.CardInfo.Subtypes.Contains("Strike");
    
}