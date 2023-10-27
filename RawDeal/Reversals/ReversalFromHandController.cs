using RawDeal.Cards;
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
    private CardController? _cardController;

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
        var numReversalToPlay = _view.AskUserToSelectAReversal(_opponentData.Name, formattedReversalPlays);
    
        if (numReversalToPlay != -1)
        {
            var selectedReversalPlay = reversalPlaysInfo[numReversalToPlay];
            ProcessSelectedReversal(selectedReversalPlay);
        }
    }

    private void ProcessSelectedReversal(IViewablePlayInfo selectedReversalPlay)
    {
        var formattedReversal = Formatter.PlayToString(selectedReversalPlay);
        selectedReversalPlay = HandleSpecialReversalDamage(selectedReversalPlay);
    
        var cardControllerType = DetermineCardControllerType(selectedReversalPlay);
        ResetChangesByJockeyingForPosition();
        CreateCardController(cardControllerType, selectedReversalPlay);
        ApplyReversalEffect(selectedReversalPlay, formattedReversal);
    }

    private CardControllerTypes DetermineCardControllerType(IViewablePlayInfo selectedReversalPlay)
    {
        var cardControllerDecider = new CardControllerDecider(selectedReversalPlay);
        return cardControllerDecider.DecideReversalCardController();
    }
    
    private IViewablePlayInfo HandleSpecialReversalDamage(IViewablePlayInfo selectedReversalPlay)
    {
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
    
    // TODO: Refactor this to eliminate functional programming from the method.
    private void CreateCardController(CardControllerTypes cardControllerType, IViewablePlayInfo selectedReversalPlay)
    {
        _cardController = cardControllerType switch
        {
            CardControllerTypes.BasicReversalCard => new BasicReversalCardController(_playerData, _opponentData,
                _selectedPlay, _view),
            CardControllerTypes.LessThanEightCard => new DoDamageReversalController(_opponentData, 
                _playerData, selectedReversalPlay, _view),
            CardControllerTypes.DoUnknownDamageCard when CheckIfSubtypeIsTheSame(selectedReversalPlay) =>
                new DoDamageReversalController(_opponentData, _playerData, selectedReversalPlay, _view),
            CardControllerTypes.PlayerDrawCard => new InterferenceReversalController(_opponentData, _playerData,
                selectedReversalPlay, _view),
            CardControllerTypes.CleanBreakReversal => new CleanBreakReversalController(_opponentData, _playerData,
                selectedReversalPlay, _view),
            CardControllerTypes.JockeyingForPosition => new JockeyingForPosition(_opponentData, _playerData,
                selectedReversalPlay, _view),
            _ => _cardController
        };
    }
    
    private void ApplyReversalEffect(IViewablePlayInfo selectedReversalPlay, string formattedReversal)
    {
        if (_cardController != null)
        {
            _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
            _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
            _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
            _cardController.ApplyEffect();
        }
    }
    
    private bool CheckIfSubtypeIsTheSame(IViewablePlayInfo selectedReversalPlay)
    {
        var reversalSubtype = selectedReversalPlay.CardInfo.Subtypes[0];
        var firstCondition = _selectedPlay.CardInfo.Subtypes.Contains("Strike") && reversalSubtype.Contains("Strike");
        var secondCondition = _selectedPlay.CardInfo.Subtypes.Contains("Grapple") && reversalSubtype.Contains("Grapple");
        return firstCondition || secondCondition;
    }
    private List<IViewablePlayInfo> CreatePlays(List<IViewableCardInfo> cardsToPlays)
    {
        var plays = new List<IViewablePlayInfo>();
        foreach (var card in cardsToPlays)
        {
            plays.Add(new PlayInfo(card, "REVERSAL"));
        }

        return plays;
    }
    
    private List<string> FormatPlays(List<IViewablePlayInfo> plays)
    {
        var formattedPlays = new List<string>();
        foreach (var play in plays)
        {
            var formattedPlay = Formatter.PlayToString(play);
            formattedPlays.Add(formattedPlay);
        }

        return formattedPlays;
    }
    
    private List<IViewableCardInfo> SearchForCorrectReversals()
    {
        var reversalCards = _opponentData.DecksController.SearchForReversalInHand();

        if (_selectedPlay.PlayedAs == "MANEUVER")
        {
            return SearchForManeuverReversals(reversalCards);
        }

        return SearchForActionReversals(reversalCards);
    }

    private List<IViewableCardInfo> SearchForManeuverReversals(IEnumerable<IViewableCardInfo> reversalCards)
    {
        var subtype = _selectedPlay.CardInfo.Subtypes[0];
        var generalReversalCards = reversalCards.Where(card => card.Subtypes[0].Contains($"{subtype}") || card.Subtypes[0].Contains($"ReversalSpecial"));

        var correctFortitudeReversalCards = CheckIfFortitudeIsHighEnough(generalReversalCards.ToList());
        var correctReversalCards = FilterLessThanEightDamageEffectCards(correctFortitudeReversalCards);

        return FilterJockeyingForPosition(correctReversalCards);
    }

    private List<IViewableCardInfo> SearchForActionReversals(IEnumerable<IViewableCardInfo> reversalCards)
    {
        var actionReversalCards = reversalCards.Where(card => card.Subtypes[0].Contains($"Action") || card.Title == "Jockeying for Position" || card.Title == "Clean Break");
        var correctFortitudeCards = CheckIfFortitudeIsHighEnough(actionReversalCards.ToList());
        var correctCards = FilterLessThanEightDamageEffectCards(correctFortitudeCards);

        return FilterJockeyingForPosition(correctCards);
    }

    
    private List<IViewableCardInfo> CheckIfFortitudeIsHighEnough(List<IViewableCardInfo> filteredCards)
    {
        var cardsWithCorrectFortitude = new List<IViewableCardInfo>();
        foreach (var filteredCard in filteredCards)
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
    
    private List<IViewableCardInfo> FilterLessThanEightDamageEffectCards(List<IViewableCardInfo> filteredCards)
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
    
    private List<IViewableCardInfo> FilterJockeyingForPosition(List<IViewableCardInfo> filteredCards)
    {
        var cardName = _selectedPlay.CardInfo.Title;
        if (cardName == "Jockeying for Position")
        {
            filteredCards.RemoveAll(card => card.Title != "Jockeying for Position" && card.Title != "Clean Break" && card.Title != "No Chance in Hell");
        }
        else
        {
            filteredCards.RemoveAll(card => card.Title == "Jockeying for Position" || card.Title == "Clean Break");
        }

        return filteredCards;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
    {
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }

    private void ResetChangesByJockeyingForPosition()
    {
        _opponentData.ChangesByJockeyingForPosition.Reset();
        _playerData.ChangesByJockeyingForPosition.Reset();
    }

}