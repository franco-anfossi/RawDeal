using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromArsenalController
{
    private View _view;
    private int _totalDamageToDo;
    private bool _damageCompleted;
    private IViewableCardInfo _drawnCard;
    private IViewablePlayInfo _selectedPlay;
    private ImportantPlayerData _playerData;
    private ImportantPlayerData _opponentData;

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
        if (CheckIfTheCardIsReversal() && CheckIfItsCorrectReversal() && CheckIfFortitudeIsHighEnough())
        {
            _view.SayThatCardWasReversedByDeck(_opponentData.Name);
            var drawnCardPlayInfo = new PlayInfo(_drawnCard, _drawnCard.Types[0].ToUpper());
            
            PlayStunValue();
            
            var cardControllerDecider = new CardControllerDecider(drawnCardPlayInfo);
            var cardControllerType = cardControllerDecider.DecideReversalCardController();
            if (cardControllerType == CardControllerTypes.BasicReversalCard)
            {
                _playerData.SuperstarData.Fortitude += Convert.ToInt32(_selectedPlay.CardInfo.Damage);
                var cardController = new BasicReversalCardController(_playerData, _opponentData, _selectedPlay, _view);
                _opponentData.DecksController.PassCardToRingside(_drawnCard);
                cardController.ApplyEffect();
            }
        }
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
    
    private bool CheckIfItsCorrectReversal()
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

    private bool CheckIfTheCardIsReversal()
    {
        return _drawnCard.Types.Contains("Reversal");
    }
    
    private bool CheckIfFortitudeIsHighEnough()
    {
        var cardFortitude = Convert.ToInt32(_drawnCard.Fortitude);
        return cardFortitude <= _opponentData.SuperstarData.Fortitude;
    }
}