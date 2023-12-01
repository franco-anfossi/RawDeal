using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Undertaker : Player
{
    private int _timesTheAbilityWasUsed;
    private const int MaxTimesTheAbilityCanBeUsed = 1;
    private const int MinCardsInHand = 2;
    private const int CardsToDiscard = 2;
    private const int CardsToDraw = 1;
    
    public Undertaker(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
            ExecuteAbilitySteps();
    }
    
    private void ExecuteAbilitySteps()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
        DiscardTwoCardsFromHand();
        DrawACardFromRingside();
        _timesTheAbilityWasUsed++;
    }
    
    private void DiscardTwoCardsFromHand()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var discardHandCardsEffect = 
            new AskToDiscardHandCardsEffect(importantPlayerData, importantPlayerData, View, CardsToDiscard);
        discardHandCardsEffect.Apply();
    }

    private void DrawACardFromRingside()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var drawCardFromRingsideEffect = new DrawFromRingsideEffect(importantPlayerData, View, CardsToDraw);
        drawCardFromRingsideEffect.Apply();
    }
    
    public override bool VerifyAbilityUsability()
        => !CanUseAbility();
    
    private bool CanUseAbility()
        => DontUseMoreThanMaxTimes() && CheckForHandWithMoreThanMinCard();
    
    private bool DontUseMoreThanMaxTimes()
        => _timesTheAbilityWasUsed < MaxTimesTheAbilityCanBeUsed;
    
    public override void ResetAbility()
    {
        if (CheckForHandWithMoreThanMinCard())
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CheckForHandWithMoreThanMinCard()
        => DecksInfo.Hand.Count >= MinCardsInHand;
}