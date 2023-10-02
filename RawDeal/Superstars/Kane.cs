using RawDealView.Formatters;

namespace RawDeal.Superstars;

public class Kane : Player
{
    public Kane(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override bool PlaySpecialAbility()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        Opponent.SayThatPlayerWillTakeDamage(1);
        
        if (!Opponent.CheckForEmptyArsenal())
            DiscardOneOpponentCard();

        return true;
    }

    private void DiscardOneOpponentCard()
    {
        IViewableCardInfo selectedCard = Opponent.PassCardFromArsenalToRingside();
        string formattedCardData = Formatter.CardToString(selectedCard);
        View.ShowCardOverturnByTakingDamage(formattedCardData, 1, 1);
    }
}