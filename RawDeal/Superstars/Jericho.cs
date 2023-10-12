using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Jericho : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public Jericho(SuperstarData superstarData) : base(superstarData)
    {
        SuperstarData = superstarData;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (DecksInfo.Hand.Count >= 1 && _timesTheAbilityWasUsed < 1)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);

            var importantPlayerData = BuildImportantPlayerData();
            
            var playerDiscardHandCardsEffect = 
                new AskToDiscardHandCardsEffect(importantPlayerData, View, 1);
            
            var opponentDiscardHandCardsEffect = 
                new AskToDiscardHandCardsEffect(OpponentData, View, 1);
            
            playerDiscardHandCardsEffect.Apply();
            opponentDiscardHandCardsEffect.Apply();
            
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        if (DecksInfo.Hand.Count >= 1 && _timesTheAbilityWasUsed < 1)
            _abilityPermission = false;
        else
            _abilityPermission = true;
        
        return _abilityPermission;
    }

    public override void ChangeAbilitySelectionVisibility()
    {
        if (DecksInfo.Hand.Count >= 1)
        {
            _timesTheAbilityWasUsed = 0;
            _abilityPermission = false;
        }
    }
}