namespace RawDeal.Superstars;

public class Undertaker : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public Undertaker(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (_timesTheAbilityWasUsed < 1 && Hand.Count >= 2)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            
            DiscardTwoCardsFromHand();
            DrawACardFromRingside();
            
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        if (Hand.Count >= 2 && _timesTheAbilityWasUsed < 1) { _abilityPermission = false; }
        else { _abilityPermission = true; }
        
        return _abilityPermission;
    }
    public override void ChangeAbilitySelectionVisibility()
    {
        if (Hand.Count >= 2)
        {
            _timesTheAbilityWasUsed = 0;
            _abilityPermission = false;
        }
    }

    private void DiscardTwoCardsFromHand()
    {
        for (int cardsToDraw = 2; cardsToDraw > 0; cardsToDraw--)
        {
            List<string> formattedCardData = Utils.FormatDecksOfCards(Hand);
            int selectedCardIndex = View.AskPlayerToSelectACardToDiscard(formattedCardData, Name, Name, cardsToDraw);
            PassCardFromHandToRingside(selectedCardIndex);
        }
    }

    private void DrawACardFromRingside()
    {
        List<string> formattedCardData = Utils.FormatDecksOfCards(Ringside);
        int selectedCardIndex = View.AskPlayerToSelectCardsToPutInHisHand(Name, 1, formattedCardData);
        PassCardFromRingsideToHand(selectedCardIndex);
    }
}