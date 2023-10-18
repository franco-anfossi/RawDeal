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
            var cardFortitude = Convert.ToInt32(selectedReversalPlay.CardInfo.Fortitude);
            var cardControllerDecider = new CardControllerDecider(selectedReversalPlay);
            var cardControllerType = cardControllerDecider.DecideReversalCardController();
            if (cardControllerType == CardControllerTypes.BasicReversalCard && cardFortitude <= _opponentData.SuperstarData.Fortitude)
            {
                var cardController = new BasicReversalCardController(_playerData, _opponentData, _selectedPlay, _view);
                var formattedReversal = Formatter.PlayToString(selectedReversalPlay);
                _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
                _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
                _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
                cardController.ApplyEffect();
            }
        }
    }
    
    private List<IViewablePlayInfo> CreatePlays(List<IViewableCardInfo> cardsToPlays)
    {
        var plays = new List<IViewablePlayInfo>();
        foreach (var card in cardsToPlays)
        {
            plays.Add(new PlayInfo(card, card.Types[0].ToUpper()));
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
            var maneuverReversalCards = reversalCards.Where(card => card.Subtypes[0].Contains($"{subtype}"));
            return CheckIfFortitudeIsHighEnough(maneuverReversalCards.ToList());
        }
        var actionReversalCards = reversalCards.Where(card => card.Subtypes[0].Contains($"Action"));
        return CheckIfFortitudeIsHighEnough(actionReversalCards.ToList());
    }
    
    private List<IViewableCardInfo> CheckIfFortitudeIsHighEnough(List<IViewableCardInfo> filteredCards)
    {
        var cardsWithCorrectFortitude = new List<IViewableCardInfo>();
        foreach (var filteredCard in filteredCards)
        {
           var cardFortitude = Convert.ToInt32(filteredCard.Fortitude);
           if (cardFortitude <= _opponentData.SuperstarData.Fortitude)
               cardsWithCorrectFortitude.Add(filteredCard);
        }

        return cardsWithCorrectFortitude;
    }
}