using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Jericho : Player
{
    private int _timesTheAbilityWasUsed;
    
    public Jericho(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            var importantPlayerData = BuildImportantPlayerData();
            ApplyAbilityEffect(importantPlayerData);
            _timesTheAbilityWasUsed++;
        }
    }
    
    private void ApplyAbilityEffect(ImportantPlayerData importantPlayerData)
    {
        var playerDiscardHandCardsEffect = 
            new AskToDiscardHandCardsEffect(importantPlayerData, importantPlayerData, View, 1);
        var opponentDiscardHandCardsEffect = new 
            AskToDiscardHandCardsEffect(OpponentData, OpponentData, View, 1);
        playerDiscardHandCardsEffect.Apply();
        opponentDiscardHandCardsEffect.Apply();
    }
    
    public override bool VerifyAbilityUsability()
    {
        return !CanUseAbility();
    }

    public override void ResetAbility()
    {
        if (!PlayerDecksController.CheckForEmptyHand())
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CanUseAbility()
    {
        return !PlayerDecksController.CheckForEmptyHand() && _timesTheAbilityWasUsed < 1;
    }
}