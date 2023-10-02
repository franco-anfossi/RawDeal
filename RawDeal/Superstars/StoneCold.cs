namespace RawDeal.Superstars;

public class StoneCold : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public StoneCold(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (_timesTheAbilityWasUsed < 1 && Arsenal.Count >= 0)
        {
            ExecuteAbilitySteps();
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        return _abilityPermission;
    }
    public override void ChangeAbilitySelectionVisibility()
    {
        _timesTheAbilityWasUsed = 0;
        _abilityPermission = false;
    }

    private void ExecuteAbilitySteps()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        View.SayThatPlayerDrawCards(Name, 1);
        DrawCard();
        List<string> formattedCardData = Utils.FormatDecksOfCards(Hand);
        int selectedCardIndex = View.AskPlayerToReturnOneCardFromHisHandToHisArsenal(Name, formattedCardData);
        PassCardFromADeckToTheBackOfTheArsenal(Hand, selectedCardIndex);
    }
}