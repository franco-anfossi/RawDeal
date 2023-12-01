using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Jericho : Player
{
    private int _timesTheAbilityWasUsed;
    private const int MaxTimesTheAbilityCanBeUsed = 1;
    private const int CardsToDiscard = 1;
    
    public Jericho(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
            ExecuteAbilitySteps();
    }

    private void ExecuteAbilitySteps()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
        var importantPlayerData = BuildImportantPlayerData();
        ApplyAbilityEffect(importantPlayerData);
        _timesTheAbilityWasUsed++;
    }
    
    private void ApplyAbilityEffect(ImportantPlayerData importantPlayerData)
    {
        var playerDiscardHandCardsEffect = BuildAbilityEffect(importantPlayerData);
        var opponentDiscardHandCardsEffect = BuildAbilityEffect(OpponentData);
        
        playerDiscardHandCardsEffect.Apply();
        opponentDiscardHandCardsEffect.Apply();
    }
    
    private Effect BuildAbilityEffect(ImportantPlayerData importantPlayerData)
        => new AskToDiscardHandCardsEffect(importantPlayerData, importantPlayerData, View, CardsToDiscard);
    
    
    public override bool VerifyAbilityUsability()
        => !CanUseAbility();

    private bool CanUseAbility()
        => !CheckForEmptyHand() && DontUseMoreThanMaxTimes();
    
    private bool DontUseMoreThanMaxTimes()
        => _timesTheAbilityWasUsed < MaxTimesTheAbilityCanBeUsed;
    
    public override void ResetAbility()
    {
        if (!CheckForEmptyHand())
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CheckForEmptyHand()
        => PlayerDecksController.CheckForEmptyHand();

}