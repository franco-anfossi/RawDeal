namespace RawDeal.Superstars;

public class TheRock : Player
{
    private bool _abilityResponse;
    public TheRock(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (Ringside.Count != 0)
        {
            _abilityResponse = View.DoesPlayerWantToUseHisAbility(Name);
            ExecuteTheRockAbility();
        }
        return true;
    }

    private void ExecuteTheRockAbility()
    {
        if (_abilityResponse)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            List<string> formattedCardData = Utils.FormatDecksOfCards(Ringside);
            int selectedCardIndex = View.AskPlayerToSelectCardsToRecover(Name, 1, formattedCardData);
            PassCardFromADeckToTheBackOfTheArsenal(Ringside, selectedCardIndex);
        }
    }
}