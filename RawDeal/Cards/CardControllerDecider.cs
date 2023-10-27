using System.Text.RegularExpressions;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardControllerDecider
{
    private readonly IViewablePlayInfo _playInfo;
    
    public CardControllerDecider(IViewablePlayInfo selectedPlay)
    {
        _playInfo = selectedPlay;
    }
    
    // TODO: Refactor this method, to eliminate the need for the CardControllerTypes enum.
    public CardControllerTypes DecideCardController()
    {
        if (CheckForBasicHybridCardsNames())
            return CardControllerTypes.BasicHybridCard;
        if (CheckForPlayerDiscardCard())
            return CardControllerTypes.PlayerDiscardCard;
        if (CheckForOpponentDiscardCard())
            return CardControllerTypes.OpponentDiscardCard;
        if (_playInfo.CardInfo.Title == "Jockeying for Position")
            return CardControllerTypes.JockeyingForPosition;
        return CardControllerTypes.BasicCard;
    }

    private bool CheckForBasicHybridCardsNames()
    {
        var nameOne = _playInfo.CardInfo.Title == "Chop";
        var nameTwo = _playInfo.CardInfo.Title == "Arm Bar Takedown";
        var nameThree = _playInfo.CardInfo.Title == "Collar & Elbow Lockup";
        return nameOne || nameTwo || nameThree;
    }

    private bool CheckForPlayerDiscardCard()
    {
        var effectPattern = @"When successfully played, discard (\d+) card(s?) of your choice from your hand";
        return Regex.IsMatch(_playInfo.CardInfo.CardEffect, effectPattern);
    }
    private bool CheckForOpponentDiscardCard()
    {
        var effectPattern = @"When successfully played, opponent must discard (\d+) card(s?)";
        return Regex.IsMatch(_playInfo.CardInfo.CardEffect, effectPattern);
    }
    
    // TODO: Refactor this method, to eliminate functional programming.
    public CardControllerTypes DecideReversalCardController()
    {
        if (CheckForNoEffectReversalCard())
        {
            return CardControllerTypes.BasicReversalCard;
        }
        if (CheckForCardsThatCanBeUsedFromSevenDOrLess() && CheckIfDoesDamageToOpponent())
        {
            return CardControllerTypes.DoUnknownDamageCard;
        }
        if (CheckForCardsThatCanBeUsedFromSevenDOrLess())
        {
            return CardControllerTypes.LessThanEightCard;
        }
        if (CheckForPlayerDrawCard())
        {
            return CardControllerTypes.PlayerDrawCard;
        }

        if (_playInfo.CardInfo.Title == "Clean Break")
        {
            return CardControllerTypes.CleanBreakReversal;
        } 
        
        if (_playInfo.CardInfo.Title == "Jockeying for Position")
        {
            return CardControllerTypes.JockeyingForPosition;
        }
        

        return CardControllerTypes.BasicCard;
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

    private bool CheckForCardsThatCanBeUsedFromSevenDOrLess()
    {
        var cardEffect = _playInfo.CardInfo.CardEffect;
        return cardEffect.Contains(" that does 7D or less.");
    }
    
    private bool CheckIfDoesDamageToOpponent()
    {
        var cardEffect = _playInfo.CardInfo.CardEffect;
        return cardEffect.Contains("# = D of maneuver card being reversed. Read as 0 when in your Ring area");
    }
    private bool CheckForPlayerDrawCard()
    {
        var effectPattern = @" draw (\d+) card(s?).";
        return Regex.IsMatch(_playInfo.CardInfo.CardEffect, effectPattern);
    }
}