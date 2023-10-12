using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Undertaker : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public Undertaker(SuperstarData superstarData) : base(superstarData)
    {
        SuperstarData = superstarData;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (_timesTheAbilityWasUsed < 1 && DecksInfo.Hand.Count >= 2)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            
            DiscardTwoCardsFromHand();
            DrawACardFromRingside();
            
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        if (DecksInfo.Hand.Count >= 2 && _timesTheAbilityWasUsed < 1) { _abilityPermission = false; }
        else { _abilityPermission = true; }
        
        return _abilityPermission;
    }
    public override void ChangeAbilitySelectionVisibility()
    {
        if (DecksInfo.Hand.Count >= 2)
        {
            _timesTheAbilityWasUsed = 0;
            _abilityPermission = false;
        }
    }

    private void DiscardTwoCardsFromHand()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var discardHandCardsEffect = 
            new AskToDiscardHandCardsEffect(importantPlayerData, View, 2);
        discardHandCardsEffect.Apply();
    }

    private void DrawACardFromRingside()
    {
        var importantPlayerData = BuildImportantPlayerData();
        var drawCardFromRingsideEffect = 
            new DrawFromRingsideEffect(importantPlayerData, View, 1);
        drawCardFromRingsideEffect.Apply();
    }
}