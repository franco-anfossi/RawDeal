using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class LessThanSevenDamage : Condition
{
    private readonly ImportantPlayerData _playerData;
    
    public LessThanSevenDamage(ImportantPlayerData playerData, IViewablePlayInfo selectedPlay) : base(selectedPlay)
    {
       _playerData = playerData;
       SelectedPlay = selectedPlay;
    }

    public override bool Check()
    {
        int damageToAdd = HandleDamageAddedByJockeyingForPosition();
        var cardDamage = Convert.ToInt32(SelectedPlay.CardInfo.Damage) + damageToAdd;
        return cardDamage <= 7;
    }
    
    private int HandleDamageAddedByJockeyingForPosition()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded = _playerData.ChangesByJockeyingForPosition.DamageAdded;
        
        return damageAdded;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
    {
        return SelectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }
    
}