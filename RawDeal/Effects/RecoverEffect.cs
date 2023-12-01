using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class RecoverEffect : Effect
{
    private readonly int _cardsToRecover;
    
    public RecoverEffect(ImportantPlayerData superstarData, View view, int cardsToRecover) : base(superstarData, view)
        => _cardsToRecover = cardsToRecover;
    
    public override void Apply()
    {
        for (int cardNum = _cardsToRecover; cardNum > 0; cardNum--)
            RecoverCard(cardNum);
    }

    private void RecoverCard(int cardNum)
    {
        var formattedDecks = PlayerData.DecksController.BuildFormattedDecks();
        var formattedRingside = formattedDecks.Ringside;
        int selectedCardIndex =
            View.AskPlayerToSelectCardsToRecover(PlayerData.Name, cardNum, formattedRingside.ToList());
        PlayerData.DecksController.PassCardFromRingsideToTheBackOfTheArsenal(selectedCardIndex);
    }

}