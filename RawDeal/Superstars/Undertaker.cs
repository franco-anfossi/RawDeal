using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Undertaker : Player
{
    private int _timesTheAbilityWasUsed;
    
    public Undertaker(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            
            DiscardTwoCardsFromHand();
            DrawACardFromRingside();
            
            _timesTheAbilityWasUsed++;
        }
    }
    
    private void DiscardTwoCardsFromHand()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var discardHandCardsEffect = 
            new AskToDiscardHandCardsEffect(importantPlayerData, importantPlayerData, View, 2);
        discardHandCardsEffect.Apply();
    }

    private void DrawACardFromRingside()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var drawCardFromRingsideEffect = new DrawFromRingsideEffect(importantPlayerData, View, 1);
        drawCardFromRingsideEffect.Apply();
    }
    
    public override bool VerifyAbilityUsability()
    {
        return !CanUseAbility();
    }
    
    public override void ResetAbility()
    {
        if (CheckForHandWithMoreThanOneCard())
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CanUseAbility()
    {
        return _timesTheAbilityWasUsed < 1 && CheckForHandWithMoreThanOneCard();
    }
    
    private bool CheckForHandWithMoreThanOneCard()
    {
        return DecksInfo.Hand.Count >= 2;
    }
}