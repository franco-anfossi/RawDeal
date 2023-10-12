using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;

namespace RawDeal.Effects;

public class DrawFromRingsideEffect : Effect
{
    private int _cardsToDraw;
    
    public DrawFromRingsideEffect(
        ImportantPlayerData superstarData, View view, int cardsToDraw) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _cardsToDraw = cardsToDraw;
    }
    
    public override void Apply()
    {
        for (int numCardsToDraw = _cardsToDraw; numCardsToDraw > 0; numCardsToDraw--)
        {
            var formattedCardData = _playerData.DecksController.BuildFormattedDecks();
            var formattedRingside = formattedCardData.Ringside;
            int selectedCardIndex = 
                _view.AskPlayerToSelectCardsToPutInHisHand(_playerData.Name, numCardsToDraw, formattedRingside);
            _playerData.DecksController.PassCardFromRingsideToHand(selectedCardIndex);
        }
    }
}