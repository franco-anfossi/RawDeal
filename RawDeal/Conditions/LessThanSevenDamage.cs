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
        int damageToAdd = HandleDamageAddedByJockeyingForPosition();
        damageToAdd += HandleDamageAddedByIrishWhip();
        int damageReduction = _playerData.BonusSet.MankindBonusDamageChange.MankindOpponentDamageChange;
        var cardDamage = Convert.ToInt32(SelectedPlay.CardInfo.Damage) + damageToAdd - damageReduction;
        return cardDamage <= 7;
    }
    
    private int HandleDamageAddedByJockeyingForPosition()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded = _playerData.BonusSet.ChangesByJockeyingForPosition.DamageAdded;
        
        return damageAdded;
    }
    
    private int HandleDamageAddedByIrishWhip()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsStrike())
            damageAdded = _playerData.BonusSet.ChangesByIrishWhip.DamageAdded;
        
        return damageAdded;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
        => SelectedPlay.CardInfo.Subtypes.Contains("Grapple");
    
    private bool CheckIfSelectedCardIsStrike()
        => SelectedPlay.CardInfo.Subtypes.Contains("Strike");
    
}