using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class LessThanSevenDamage : Condition
{
    private readonly ImportantPlayerData _playerData;
    private readonly IViewablePlayInfo _selectedPlay;
    
    public LessThanSevenDamage(ImportantPlayerData playerData, IViewablePlayInfo selectedPlay)
    {
       _playerData = playerData;
       _selectedPlay = selectedPlay;
    }

    public override bool Check()
    {
        int damageToAdd = HandleDamageAddedByJockeyingForPosition();
        var cardDamage = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
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
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }
    
}