using RawDeal.Boundaries;
using RawDeal.Cards;
using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromHandController
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private CardController _cardController;

    public ReversalFromHandController(ImportantPlayerData playerData, 
        ImportantPlayerData opponentData, IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _playerData = playerData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }

    public void SelectReversalFromHand()
    {
        var correctReversalCards = SearchForCorrectReversals();
        var reversalPlaysInfo = CreatePlays(correctReversalCards);
        var formattedReversalPlays = FormatPlays(reversalPlaysInfo);
        var numReversalToPlay = 
            _view.AskUserToSelectAReversal(_opponentData.Name, formattedReversalPlays.ToList());
    
        if (numReversalToPlay != -1)
        {
            var selectedReversalPlay = reversalPlaysInfo[numReversalToPlay];
            ProcessSelectedReversal(selectedReversalPlay);
        }
    }
    
    private BoundaryList<IViewableCardInfo> SearchForCorrectReversals()
    {
        var reversalCards = _opponentData.DecksController.SearchForReversalInHand();

        if (_selectedPlay.PlayedAs == "MANEUVER")
            return SearchForManeuverReversals(reversalCards);

        return SearchForActionReversals(reversalCards);
    }
    
    private BoundaryList<IViewablePlayInfo> CreatePlays(BoundaryList<IViewableCardInfo> cardsToPlays)
    {
        var plays = new BoundaryList<IViewablePlayInfo>();
        foreach (var card in cardsToPlays)
        {
            plays.Add(new PlayInfo(card, "REVERSAL"));
        }

        return plays;
    }
    
    private BoundaryList<string> FormatPlays(BoundaryList<IViewablePlayInfo> plays)
    {
        var formattedPlays = new BoundaryList<string>();
        foreach (var play in plays)
        {
            var formattedPlay = Formatter.PlayToString(play);
            formattedPlays.Add(formattedPlay);
        }

        return formattedPlays;
    }

    private void ProcessSelectedReversal(IViewablePlayInfo selectedReversalPlay)
    {
        var formattedReversal = FormatReversal(selectedReversalPlay);
        selectedReversalPlay = HandleSpecialReversalDamage(selectedReversalPlay);
        
        BuildCardController(selectedReversalPlay);
        ResetChangesByJockeyingForPosition();
    
        if (_cardController.CheckConditions())
        {
            ApplyReversalEffect(selectedReversalPlay, formattedReversal);
        }
    }

    private string FormatReversal(IViewablePlayInfo reversalPlay)
    {
        return Formatter.PlayToString(reversalPlay);
    }
    
    private IViewablePlayInfo HandleSpecialReversalDamage(IViewablePlayInfo selectedReversalPlay)
    {
        // TODO: Encapsulate this into a method
        if (selectedReversalPlay.CardInfo.Damage == "#")
        {
            int damageToAdd = HandleDamageAddedByJockeyingForPosition();
            int damageValue = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;

            if (_opponentData.Name == "MANKIND")
                damageValue -= 1;

            selectedReversalPlay.CardInfo.Damage = "#" + damageValue;
        }

        return selectedReversalPlay;
    }

    private void BuildCardController(IViewablePlayInfo reversalPlay)
    {
        var lastCardUsed = new LastCardUsed(_selectedPlay);
        
        var conditionBuilder = new ConditionBuilder(_playerData, _selectedPlay, reversalPlay, lastCardUsed);
        var conditions = conditionBuilder.BuildConditions();
        
        var effectBuilder = new ReversalEffectBuilder(_opponentData, 
            _playerData, reversalPlay, ReversalPlayedFrom.PlayedFromHand, _view);
        var effects = effectBuilder.BuildEffects();
        
        _cardController = new CardController(effects, conditions);
    }
    
    private void ResetChangesByJockeyingForPosition()
    {
        _opponentData.ChangesByJockeyingForPosition.Reset();
        _playerData.ChangesByJockeyingForPosition.Reset();
    }
    
    private void ApplyReversalEffect(IViewablePlayInfo selectedReversalPlay, string formattedReversal)
    {
        _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
        _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
        _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
        _cardController.PlayCard();
    }

    private BoundaryList<IViewableCardInfo> SearchForManeuverReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        var subtype = _selectedPlay.CardInfo.Subtypes[0];
        var generalReversalCards = reversalCards.Where(card => 
            card.Subtypes[0].Contains($"{subtype}") || card.Subtypes[0].Contains($"ReversalSpecial")); // TODO: Encapsulate

        var correctFortitudeReversalCards = 
            CheckIfFortitudeIsHighEnough(generalReversalCards.ToBoundaryList());
        
        var correctReversalCards = 
            FilterLessThanEightDamageEffectCards(correctFortitudeReversalCards);

        return FilterJockeyingForPosition(correctReversalCards);
    }

    private BoundaryList<IViewableCardInfo> SearchForActionReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        var actionReversalCards = reversalCards.Where(card => 
            card.Subtypes[0].Contains($"Action") || card.Title == "Jockeying for Position" || 
            card.Title == "Clean Break"); // TODO: Encapsulate
        
        var correctFortitudeCards = 
            CheckIfFortitudeIsHighEnough(actionReversalCards.ToBoundaryList());
        
        var correctCards = FilterLessThanEightDamageEffectCards(correctFortitudeCards);

        return FilterJockeyingForPosition(correctCards);
    }

    
    private BoundaryList<IViewableCardInfo> CheckIfFortitudeIsHighEnough(BoundaryList<IViewableCardInfo> filteredCards)
    {
        var cardsWithCorrectFortitude = new BoundaryList<IViewableCardInfo>();
        foreach (IViewableCardInfo filteredCard in filteredCards)
        {
            int fortitudeToAdd = HandleFortitudeAddedByJockeyingForPosition();
            var cardFortitude = Convert.ToInt32(filteredCard.Fortitude);
            if (cardFortitude + fortitudeToAdd <= _opponentData.SuperstarData.Fortitude) 
                cardsWithCorrectFortitude.Add(filteredCard);
        }
        
        return cardsWithCorrectFortitude;
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
    
    private BoundaryList<IViewableCardInfo> FilterLessThanEightDamageEffectCards(
        BoundaryList<IViewableCardInfo> filteredCards)
    {
        int damageToAdd = HandleDamageAddedByJockeyingForPosition();
        
        var cardDamage = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
        if (cardDamage > 7)
        {
            filteredCards.RemoveAll(card => card.CardEffect.Contains("that does 7D or less."));
        }
        return filteredCards;
    }
    
    private int HandleDamageAddedByJockeyingForPosition()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded = _playerData.ChangesByJockeyingForPosition.DamageAdded;
        
        return damageAdded;
    }
    
    private BoundaryList<IViewableCardInfo> FilterJockeyingForPosition(BoundaryList<IViewableCardInfo> filteredCards)
    {
        var cardName = _selectedPlay.CardInfo.Title;
        if (cardName == "Jockeying for Position")
        {
            filteredCards.RemoveAll(card => card.Title != "Jockeying for Position" && 
                                            card.Title != "Clean Break" && card.Title != "No Chance in Hell"); // TODO: Encapsulate
        }
        else
        {
            filteredCards.RemoveAll(card => card.Title == "Jockeying for Position" || 
                                            card.Title == "Clean Break"); // TODO: Encapsulate
        }

        return filteredCards;
    }

    private bool CheckIfSelectedCardIsGrapple()
    {
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }
}