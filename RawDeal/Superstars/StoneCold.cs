using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class StoneCold : Player
{
    private int _timesTheAbilityWasUsed;
    private const int CardsToDraw = 1;
    private const int MaxTimesTheAbilityCanBeUsed = 1;
    
    public StoneCold(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            View.SayThatPlayerDrawCards(SuperstarData.Name, CardsToDraw);
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
        => !CanUseAbility();
    
    private bool CanUseAbility()
        => !PlayerDecksController.CheckForEmptyArsenal() && _timesTheAbilityWasUsed < MaxTimesTheAbilityCanBeUsed;
    
    public override void ResetAbility()
    {
        if (!PlayerDecksController.CheckForEmptyArsenal())  
            _timesTheAbilityWasUsed = 0;
    }
}