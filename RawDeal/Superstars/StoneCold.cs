using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class StoneCold : Player
{
    private int _timesTheAbilityWasUsed;
    
    public StoneCold(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            View.SayThatPlayerDrawCards(SuperstarData.Name, 1);
            ExecuteAbilitySteps();
            _timesTheAbilityWasUsed++;
        }
    }
    
    private void ExecuteAbilitySteps()
    {
        PlayerDecksController.DrawTurnCard();
        var importantPlayerData = BuildImportantPlayerData();
        var returnCardEffect = new ReturnCardToArsenalEffect(importantPlayerData, View);
        returnCardEffect.Apply();
    }
    
    public override bool VerifyAbilityUsability()
    {
        return !CanUseAbility();
    }
    
    public override void ResetAbility()
    {
        if (!PlayerDecksController.CheckForEmptyArsenal())  
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CanUseAbility()
    {
        return !PlayerDecksController.CheckForEmptyArsenal() && _timesTheAbilityWasUsed < 1;
    }
}