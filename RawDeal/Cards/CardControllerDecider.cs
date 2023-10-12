using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardControllerDecider
{
    private IViewableCardInfo _cardInfo;
    
    public CardControllerDecider(IViewablePlayInfo selectedPlay)
    {
        _cardInfo = selectedPlay.CardInfo;
    }

    private bool CheckForBasicHybridCardsNames()
    {
        var nameOne = _cardInfo.Title == "Chop";
        var nameTwo = _cardInfo.Title == "Arm Bar Takedown";
        var nameThree = _cardInfo.Title == "Collar & Elbow Lockup";
        return nameOne || nameTwo || nameThree;
    }

    private bool CheckForHybridCard()
    {
        var cardTypes = _cardInfo.Types;
        var isAction = cardTypes.Contains("Action");
        var isManeuver = cardTypes.Contains("Maneuver");
        return isAction && isManeuver;
    }
    
    public CardControllerTypes DecideCardController()
    {
        if (CheckForBasicHybridCardsNames() && CheckForHybridCard())
        {
            return CardControllerTypes.BasicHybridCard;
        }
        return CardControllerTypes.BasicCard;
    }
}