using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class RecoverEffect : Effect
{
    private readonly int _cardsToRecover;
    
    public RecoverEffect(ImportantPlayerData superstarData, View view, int cardsToRecover) : base(superstarData, view)
    {
        View = view;
        PlayerData = superstarData;
        _cardsToRecover = cardsToRecover;
    }
    
    public override void Apply()
    {
        var formattedCardData = PlayerData.DecksController.BuildFormattedDecks();
        var formattedRingside = formattedCardData.Ringside;
        int selectedCardIndex = 
            View.AskPlayerToSelectCardsToRecover(PlayerData.Name, _cardsToRecover, formattedRingside);
        PlayerData.DecksController.PassCardFromRingsideToTheBackOfTheArsenal(selectedCardIndex);
    }
}