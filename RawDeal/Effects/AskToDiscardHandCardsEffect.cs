using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class AskToDiscardHandCardsEffect : Effect
{
    private readonly int _cardsToDiscard;
    private readonly ImportantPlayerData _playerDeciderData;
    
    public AskToDiscardHandCardsEffect(ImportantPlayerData superstarData, ImportantPlayerData playerDeciderData, 
        View view, int cardsToDiscard) : base(superstarData, view)
    {
        _playerDeciderData = playerDeciderData;
        _cardsToDiscard = cardsToDiscard;
    }
    
    public override void Apply()
    {
        var playerDecks = PlayerData.DecksController.BuildDecks();
        int numberOfCardsToDiscard = Math.Min(playerDecks.Hand.Count, _cardsToDiscard);
        
        int remainingCardsToDiscard = _cardsToDiscard;

        for (int i = 0; i < numberOfCardsToDiscard; i++)
        {
            int selectedCardIndex = ChooseCardToDiscard(remainingCardsToDiscard);
            PassCardToRingside(selectedCardIndex);
            remainingCardsToDiscard--;
        }
    }
    
    private int ChooseCardToDiscard(int cardsToDraw)
    {
        var formattedCardData = PlayerData.DecksController.BuildFormattedDecks();
        var formattedHand = formattedCardData.Hand;
        int selectedCardIndex = View.AskPlayerToSelectACardToDiscard(formattedHand.ToList(), 
            PlayerData.Name, _playerDeciderData.Name, cardsToDraw);
        return selectedCardIndex;
    }
    
    private void PassCardToRingside(int selectedCardIndex)
    {
        var cardData = PlayerData.DecksController.BuildDecks();
        var selectedCard = cardData.Hand[selectedCardIndex];
        PlayerData.DecksController.PassCardFromHandToRingside(selectedCard);
    }
}