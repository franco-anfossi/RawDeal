using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromHandController
{
    private View _view;
    private IViewablePlayInfo _selectedPlay;
    private ImportantPlayerData _playerData;
    private ImportantPlayerData _opponentData;

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
            var formattedReversal = Formatter.PlayToString(selectedReversalPlay);
            if (selectedReversalPlay.CardInfo.Damage == "#")
            {
                int damageToAdd = 0;
                if (_selectedPlay.CardInfo.Subtypes.Contains("Grapple"))
                {
                    damageToAdd = _playerData.ChangesByJockeyingForPosition.DamageAdded;
                }
                
                int damageValue = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
                if (_opponentData.Name == "MANKIND")
                    damageValue -= 1;

                selectedReversalPlay.CardInfo.Damage = "#" + damageValue;
            }
            var cardControllerDecider = new CardControllerDecider(selectedReversalPlay);
            var cardControllerType = cardControllerDecider.DecideReversalCardController();
            _opponentData.ChangesByJockeyingForPosition.Reset();
            _playerData.ChangesByJockeyingForPosition.Reset();
            if (cardControllerType == (CardControllerTypes.BasicReversalCard))
            {
                var cardController = new BasicReversalCardController(_playerData, _opponentData, _selectedPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
            else if (cardControllerType == CardControllerTypes.LessThanEightCard)
            {
                var cardController = new DoDamageReversalController(
                    _opponentData, _playerData, selectedReversalPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
            else if (cardControllerType == CardControllerTypes.DoUnknownDamageCard && CheckIfSubtypeIsTheSame(selectedReversalPlay))
            {
                var cardController = new DoDamageReversalController(_opponentData,
                    _playerData, selectedReversalPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
            else if (cardControllerType == CardControllerTypes.PlayerDrawCard)
            {
                var cardController = new InterferenceReversalController(_opponentData,
                    _playerData, selectedReversalPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
            else if (cardControllerType == CardControllerTypes.CleanBreakReversal)
            {
                var cardController = new CleanBreakReversalController(_opponentData,
                    _playerData, selectedReversalPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
            else if (cardControllerType == CardControllerTypes.JockeyingForPosition)
            {
                var cardController = new JockeyingForPosition(_opponentData,
                    _playerData, selectedReversalPlay, _view);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
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
            var subtype = _selectedPlay.CardInfo.Subtypes[0];
            var generalReversalCards = reversalCards.Where(card => card.Subtypes[0].Contains($"{subtype}") || card.Subtypes[0].Contains($"ReversalSpecial"));
            var correctFortitudeReversalCards = CheckIfFortitudeIsHighEnough(generalReversalCards.ToList());
            var correctReversalCards = FilterLessThanEightDamageEffectCards(correctFortitudeReversalCards);
            var finalCorrectCards = FilterJockeyingForPosition(correctReversalCards);
            return finalCorrectCards;
        }
        var actionReversalCards = reversalCards.Where(card =>
            card.Subtypes[0].Contains($"Action") || card.Title == "Jockeying for Position" || card.Title == "Clean Break");
        var correctFortitudeCards = CheckIfFortitudeIsHighEnough(actionReversalCards.ToList());
        var correctCards = FilterLessThanEightDamageEffectCards(correctFortitudeCards);
        return FilterJockeyingForPosition(correctCards);
    }
    
    private List<IViewableCardInfo> CheckIfFortitudeIsHighEnough(List<IViewableCardInfo> filteredCards)
    {
        var cardsWithCorrectFortitude = new List<IViewableCardInfo>();
        foreach (var filteredCard in filteredCards)
        {
            int fortitudeToAdd = 0;
            if (_selectedPlay.CardInfo.Subtypes.Contains("Grapple"))
            {
                fortitudeToAdd = _opponentData.ChangesByJockeyingForPosition.FortitudeNeeded;
            }
            
            var cardFortitude = Convert.ToInt32(filteredCard.Fortitude);
            if (cardFortitude + fortitudeToAdd <= _opponentData.SuperstarData.Fortitude) 
                cardsWithCorrectFortitude.Add(filteredCard);
        }
        
        return cardsWithCorrectFortitude;
    }
    
    private List<IViewableCardInfo> FilterLessThanEightDamageEffectCards(List<IViewableCardInfo> filteredCards)
    {
        int damageToAdd = 0;
        if (_selectedPlay.CardInfo.Subtypes.Contains("Grapple"))
        {
            damageToAdd = _playerData.ChangesByJockeyingForPosition.DamageAdded;
        }
        
        var cardDamage = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
        if (cardDamage > 7)
        {
            filteredCards.RemoveAll(card => card.CardEffect.Contains("that does 7D or less."));
        }
        return filteredCards;
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

}