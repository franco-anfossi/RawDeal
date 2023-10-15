using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class AskToDiscardHandCardsEffect : Effect
{
    private readonly int _cardsToDiscard;
    
    public AskToDiscardHandCardsEffect(
        ImportantPlayerData superstarData, View view, int cardsToDiscard) : base(superstarData, view)
    {
        View = view;
        PlayerData = superstarData;
        _cardsToDiscard = cardsToDiscard;
    }
    
    public override void Apply()
    {
        for (int cardsToDraw = _cardsToDiscard; cardsToDraw > 0; cardsToDraw--)
        {
            var formattedCardData = PlayerData.DecksController.BuildFormattedDecks();
            var formattedHand = formattedCardData.Hand;
            int selectedCardIndex = View.AskPlayerToSelectACardToDiscard(formattedHand, 
                PlayerData.Name, PlayerData.Name, cardsToDraw);
            var cardData = PlayerData.DecksController.BuildDecks();
            var selectedCard = cardData.Hand[selectedCardIndex];
            
            PlayerData.DecksController.PassCardFromHandToRingside(selectedCard);
        }
    }
}