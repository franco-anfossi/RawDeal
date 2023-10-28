using RawDeal.Boundaries;
using RawDeal.Cards;
using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromArsenalController
{
    private readonly View _view;
    private readonly StunValueCondition _damageCompleted;
    private readonly IViewableCardInfo _drawnCard;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private CardController _cardController;

    public ReversalFromArsenalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, IViewableCardInfo drawnCard, StunValueCondition damageCompleted, View view)
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
        if (CheckIfPlayableReversal())
        {
            if (CheckCardConditions())
                ExecuteArsenalReversalAction();
        }
    }

    private bool CheckIfPlayableReversal()
    {
        return CheckIfTheCardIsReversal() && CheckIfFortitudeIsHighEnough();
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
    
    private bool CheckIfSelectedCardIsGrapple()
    {
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }
    
    private bool CheckCardConditions()
    {
        var drawnCardPlayInfo = CreateDrawnCardPlayInfo();
        BuildCardController(drawnCardPlayInfo);
        
        return _cardController.CheckConditions();
    }

    private IViewablePlayInfo CreateDrawnCardPlayInfo()
    {
        return new PlayInfo(_drawnCard, _drawnCard.Types[0].ToUpper());
    }

    private void BuildCardController(IViewablePlayInfo drawnCardPlayInfo)
    {
        var conditionBuilder = new ConditionBuilder(_playerData, _selectedPlay, drawnCardPlayInfo);
        var conditions = conditionBuilder.BuildConditions();
        
        var effectBuilder = new ReversalEffectBuilder(_opponentData, 
            _playerData, _selectedPlay, ReversalPlayedFrom.PlayedFromArsenal, _view);
        var effects = effectBuilder.BuildEffects();
        
        _cardController = new CardController(effects, conditions);
    }
    
    private void ExecuteArsenalReversalAction()
    {
        ResetChangesByJockeyingForPosition();
        _view.SayThatCardWasReversedByDeck(_opponentData.Name);
        PlayStunValue();
        _playerData.SuperstarData.Fortitude += Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        _opponentData.DecksController.PassCardToRingside(_drawnCard);
        _cardController.PlayCard();
    }

    private void ResetChangesByJockeyingForPosition()
    {
        _playerData.ChangesByJockeyingForPosition.Reset();
        _opponentData.ChangesByJockeyingForPosition.Reset();
    }
    
    private void PlayStunValue()
    {
        var stunValueNumber = Convert.ToInt32(_selectedPlay.CardInfo.StunValue);
        if (CheckForExecutableStunValue(stunValueNumber))
        {
            var cardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(_playerData.Name, stunValueNumber);
            for (int i = 0; i < cardsToDraw; i++)
            {
                _playerData.DecksController.DrawCard();
            }
            _view.SayThatPlayerDrawCards(_playerData.Name, cardsToDraw);
        }
    }
    
    private bool CheckForExecutableStunValue(int stunValueNumber)
    {
        return _damageCompleted == StunValueCondition.DamageNotCompleted && stunValueNumber > 0;
    }
}