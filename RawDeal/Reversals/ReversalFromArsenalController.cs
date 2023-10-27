using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromArsenalController
{
    private readonly View _view;
    private readonly bool _damageCompleted;
    private readonly IViewableCardInfo _drawnCard;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;

    public ReversalFromArsenalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, IViewableCardInfo drawnCard, bool damageCompleted, View view)
    {
        _view = view;
        _drawnCard = drawnCard;
        _playerData = playerData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
        _damageCompleted = damageCompleted;
    }

    public void ReviewIfReversalPlayable()
    {
        if (CheckIfTheCardIsReversal() && CheckIfFortitudeIsHighEnough())
        {
            var drawnCardPlayInfo = new PlayInfo(_drawnCard, _drawnCard.Types[0].ToUpper());
            var cardControllerDecider = new CardControllerDecider(drawnCardPlayInfo);
            var cardControllerType = cardControllerDecider.DecideReversalCardController();
            ChooseArsenalReversalCard(cardControllerType, drawnCardPlayInfo);
        }
    }
    
    private bool CheckIfTheCardIsReversal()
    {
        return _drawnCard.Types.Contains("Reversal");
    }
    
    private bool CheckIfFortitudeIsHighEnough()
    {
        int fortitudeToAdd = HandleFortitudeAddedByJockeyingForPosition();
        var cardFortitude = Convert.ToInt32(_drawnCard.Fortitude);
        return cardFortitude + fortitudeToAdd <= _opponentData.SuperstarData.Fortitude;
    }
    
    private int HandleFortitudeAddedByJockeyingForPosition()
    {
        int fortitudeAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
        {
            fortitudeAdded = _opponentData.ChangesByJockeyingForPosition.FortitudeNeeded;
        }
        return fortitudeAdded;
    }
    
    // TODO: Refactor this to eliminate functional programming from the method.
    private void ChooseArsenalReversalCard(CardControllerTypes cardControllerType, IViewablePlayInfo drawnCardPlayInfo)
    {
        switch (cardControllerType)
        {
            case CardControllerTypes.PlayerDrawCard:
            case CardControllerTypes.LessThanEightCard when CheckIfCardMakesLessThanEight():
            case CardControllerTypes.BasicReversalCard when CheckIfItsCorrectBasicReversal():
            case CardControllerTypes.CleanBreakReversal when CheckIfCardIsJockeyingForPosition():
            case CardControllerTypes.JockeyingForPosition when CheckIfCardIsJockeyingForPosition():
            case CardControllerTypes.DoUnknownDamageCard 
                when CheckIfCardMakesLessThanEight() && CheckIfSubtypeIsTheSame(drawnCardPlayInfo):
                ExecuteArsenalReversalAction();
                break;
        }
    }
    
    private void ExecuteArsenalReversalAction()
    {
        ResetChangesByJockeyingForPosition();
        _view.SayThatCardWasReversedByDeck(_opponentData.Name);
        PlayStunValue();
        _playerData.SuperstarData.Fortitude += Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        _opponentData.DecksController.PassCardToRingside(_drawnCard);
        throw new EndOfTurnException();
    }

    
    private bool CheckIfSubtypeIsTheSame(IViewablePlayInfo drawnCardPlayInfo)
    {
        var reversalSubtype = drawnCardPlayInfo.CardInfo.Subtypes[0];
        var firstCondition = _selectedPlay.CardInfo.Subtypes.Contains("Strike") && reversalSubtype.Contains("Strike");
        var secondCondition = _selectedPlay.CardInfo.Subtypes.Contains("Grapple") && reversalSubtype.Contains("Grapple");
        return firstCondition || secondCondition;
    }

    private void ResetChangesByJockeyingForPosition()
    {
        _playerData.ChangesByJockeyingForPosition.Reset();
        _opponentData.ChangesByJockeyingForPosition.Reset();
    }
    
    private void PlayStunValue()
    {
        var stunValueNumber = Convert.ToInt32(_selectedPlay.CardInfo.StunValue);
        if (!_damageCompleted && stunValueNumber > 0)
        {
            var cardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(_playerData.Name, stunValueNumber);
            for (int i = 0; i < cardsToDraw; i++)
            {
                _playerData.DecksController.DrawCard();
            }
            _view.SayThatPlayerDrawCards(_playerData.Name, cardsToDraw);
        }
    }
    
    private bool CheckIfItsCorrectBasicReversal()
    {
        if (_selectedPlay.PlayedAs == "MANEUVER")
        {
            var subtype = _selectedPlay.CardInfo.Subtypes[0];
            var maneuverReversalCards = _drawnCard.Subtypes[0].Contains($"{subtype}");
            return maneuverReversalCards;
        }
        var actionReversalCards = _drawnCard.Subtypes[0].Contains($"Action");
        return actionReversalCards;
    }
    
    private bool CheckIfCardMakesLessThanEight()
    {
        int damageToAdd = HandleDamageAddedByJockeyingForPosition();
        var cardDamage = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
        return cardDamage <= 7;
    }
    
    private int HandleDamageAddedByJockeyingForPosition()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded = _playerData.ChangesByJockeyingForPosition.DamageAdded;
        
        return damageAdded;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
    {
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }

    private bool CheckIfCardIsJockeyingForPosition()
    {
        return _selectedPlay.CardInfo.Title == "Jockeying for Position";
    }
}