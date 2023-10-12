using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;

namespace RawDeal.Effects;

public class AskToDiscardHandCardsEffect : Effect
{
    private int _cardsToDiscard;
    
    public AskToDiscardHandCardsEffect(
        ImportantPlayerData superstarData, View view, int cardsToDiscard) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _cardsToDiscard = cardsToDiscard;
    }
    
    public override void Apply()
    {
        for (int cardsToDraw = _cardsToDiscard; cardsToDraw > 0; cardsToDraw--)
        {
            var formattedCardData = _playerData.DecksController.BuildFormattedDecks();
            var formattedHand = formattedCardData.Hand;
            int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(formattedHand, 
                _playerData.Name, _playerData.Name, cardsToDraw);
            var cardData = _playerData.DecksController.BuildDecks();
            var selectedCard = cardData.Hand[selectedCardIndex];
            
            _playerData.DecksController.PassCardFromHandToRingside(selectedCard);
        }
    }
}