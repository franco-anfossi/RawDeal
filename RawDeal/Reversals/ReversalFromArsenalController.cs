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
    
    public bool CheckPreconditions()
    {
        var reversalPreconditionBuilder = new ReversalPreconditionBuilder(_selectedPlay, _opponentData);
        var reversalPreconditions = reversalPreconditionBuilder.BuildConditions();

        return reversalPreconditions.All(condition => condition.Check());
    }

    public void ReviewIfReversalPlayable()
    {
        if (CheckIfPlayableReversal() && CheckCardConditions())
            ExecuteArsenalReversalAction();
    }
    
    private bool CheckCardConditions()
    {
        var drawnCardPlayInfo = CreateDrawnCardPlayInfo();
        BuildCardController(drawnCardPlayInfo);
        
        return _cardController.CheckConditions();
    }
    
    private IViewablePlayInfo CreateDrawnCardPlayInfo()
        => new PlayInfo(_drawnCard, _drawnCard.Types[0].ToUpper());
    
    private void BuildCardController(IViewablePlayInfo drawnCardPlayInfo)
    {
        var conditionBuilder = new ConditionBuilder(_playerData, _selectedPlay, drawnCardPlayInfo);
        var conditions = conditionBuilder.BuildConditions();
        
        var effectBuilder = new ReversalEffectBuilder(_opponentData, 
            _playerData, _selectedPlay, ReversalPlayedFrom.PlayedFromArsenal, _view);
        var effects = effectBuilder.BuildEffects();
        
        _cardController = new CardController(effects, conditions);
    }
    
    private bool CheckIfPlayableReversal()
        => CheckIfTheCardIsReversal() && CheckIfFortitudeIsHighEnough();

    private bool CheckIfTheCardIsReversal()
        => _drawnCard.Types.Contains("Reversal");
    
    private bool CheckIfFortitudeIsHighEnough()
    {
        int fortitudeToAdd = HandleFortitudeAddedByCards();
        var cardFortitude = Convert.ToInt32(_drawnCard.Fortitude);
        return cardFortitude + fortitudeToAdd <= _opponentData.SuperstarData.Fortitude;
    }
    
    private int HandleFortitudeAddedByCards()
    {
        int fortitudeAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            fortitudeAdded = _opponentData.BonusSet.ChangesByJockeyingForPosition.FortitudeNeeded;
        
        return fortitudeAdded;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
        => _selectedPlay.CardInfo.Subtypes.Contains("Grapple");

    private void ExecuteArsenalReversalAction()
    {
        ResetChangesByCards();
        _view.SayThatCardWasReversedByDeck(_opponentData.Name);
        PlayStunValue();
        _playerData.SuperstarData.Fortitude += Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        _opponentData.DecksController.PassCardToRingside(_drawnCard);
        _cardController.PlayCard();
    }

    private void ResetChangesByCards()
    {
        _playerData.BonusSet.ChangesByJockeyingForPosition.Reset();
        _opponentData.BonusSet.ChangesByJockeyingForPosition.Reset();
        
        _playerData.BonusSet.ChangesByIrishWhip.Reset();
        _opponentData.BonusSet.ChangesByIrishWhip.Reset();
    }
    
    private void PlayStunValue()
    {
        var stunValueNumber = Convert.ToInt32(_selectedPlay.CardInfo.StunValue);
        if (CheckForExecutableStunValue(stunValueNumber))
            DrawCardsBecauseOfStunValue(stunValueNumber);
    }
    
    private bool CheckForExecutableStunValue(int stunValueNumber)
        => _damageCompleted == StunValueCondition.DamageNotCompleted && stunValueNumber > 0;
    
    private void DrawCardsBecauseOfStunValue(int stunValueNumber)
    {
        var cardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(_playerData.Name, stunValueNumber);
        for (int i = 0; i < cardsToDraw; i++)
            _playerData.DecksController.DrawCard();
            
        _view.SayThatPlayerDrawCards(_playerData.Name, cardsToDraw);
    }
}