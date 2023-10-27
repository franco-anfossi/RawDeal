using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class RecoverEffect : Effect
{
    private readonly int _cardsToRecover;
    
    public RecoverEffect(ImportantPlayerData superstarData, View view, int cardsToRecover) : base(superstarData, view)
    {
        _cardsToRecover = cardsToRecover;
    }
    
    public override void Apply()
    {
        var playerName = PlayerData.Name;
        var formattedDecks = PlayerData.DecksController.BuildFormattedDecks();
        var formattedRingside = formattedDecks.Ringside;
        int selectedCardIndex = View.AskPlayerToSelectCardsToRecover(playerName, _cardsToRecover, formattedRingside);
        PlayerData.DecksController.PassCardFromRingsideToTheBackOfTheArsenal(selectedCardIndex);
    }
}