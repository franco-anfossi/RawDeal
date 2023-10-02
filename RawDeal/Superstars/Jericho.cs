namespace RawDeal.Superstars;

public class Jericho : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public Jericho(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (Hand.Count >= 1 && _timesTheAbilityWasUsed < 1)
        {
            DiscardOneCardFromJerichoHand();
            DiscardOneCardFromOpponentHand();
            
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        if (Hand.Count >= 1 && _timesTheAbilityWasUsed < 1)
        {
            _abilityPermission = false;
        }
        else
        {
            _abilityPermission = true;
        }
        
        return _abilityPermission;
    }

    public override void ChangeAbilitySelectionVisibility()
    {
        if (Hand.Count >= 1)
        {
            _timesTheAbilityWasUsed = 0;
            _abilityPermission = false;
        }
    }

    private void DiscardOneCardFromJerichoHand()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        List<string> formattedCardData = Utils.FormatDecksOfCards(Hand);
        int selectedCardIndex = View.AskPlayerToSelectACardToDiscard(formattedCardData, Name, Name, 1);
        PassCardFromHandToRingside(selectedCardIndex);
    }

    private void DiscardOneCardFromOpponentHand()
    {
        List<string> formattedCardData = Utils.FormatDecksOfCards(Opponent.GetHand());
        int selectedCardIndex = Opponent.AskForCardsToDiscard(formattedCardData, 1);
        Opponent.PassCardFromHandToRingside(selectedCardIndex);
    }
}