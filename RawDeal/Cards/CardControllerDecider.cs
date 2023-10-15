using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardControllerDecider
{
    private readonly IViewablePlayInfo _playInfo;
    
    public CardControllerDecider(IViewablePlayInfo selectedPlay)
    {
        _playInfo = selectedPlay;
    }
    
    public CardControllerTypes DecideCardController()
    {
        if (CheckForBasicHybridCardsNames() && CheckForHybridCard())
        {
            return CardControllerTypes.BasicHybridCard;
        }
        return CardControllerTypes.BasicCard;
    }
    private bool CheckForBasicHybridCardsNames()
    {
        var nameOne = _playInfo.CardInfo.Title == "Chop";
        var nameTwo = _playInfo.CardInfo.Title == "Arm Bar Takedown";
        var nameThree = _playInfo.CardInfo.Title == "Collar & Elbow Lockup";
        return nameOne || nameTwo || nameThree;
    }
    
    private bool CheckForHybridCard()
    {
        var cardTypes = _playInfo.CardInfo.Types;
        var isAction = cardTypes.Contains("Action");
        var isManeuver = cardTypes.Contains("Maneuver");
        return isAction && isManeuver;
    }
    
    public CardControllerTypes DecideReversalCardController()
    {
        return CheckForNoEffectReversalCard() ? CardControllerTypes.BasicReversalCard : CardControllerTypes.BasicCard;
    }
    
    private bool CheckForNoEffectReversalCard()
    {
        var cardSubtypes = _playInfo.CardInfo.Subtypes;
        var isGrappleReversal = cardSubtypes.Contains("ReversalGrapple");
        var isStrikeReversal = cardSubtypes.Contains("ReversalStrike");
        var isSubmissionReversal = cardSubtypes.Contains("ReversalSubmission");
        var isActionReversal = cardSubtypes.Contains("ReversalAction");
        
        return isGrappleReversal || isStrikeReversal || isSubmissionReversal || isActionReversal;
    }
    
}