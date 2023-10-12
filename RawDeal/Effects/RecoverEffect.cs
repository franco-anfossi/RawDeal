using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;

namespace RawDeal.Effects;

public class RecoverEffect : Effect
{
    private int _cardsToRecover;
    
    public RecoverEffect(ImportantPlayerData superstarData, View view, int cardsToRecover) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _cardsToRecover = cardsToRecover;
    }
    
    public override void Apply()
    {
        var formattedCardData = _playerData.DecksController.BuildFormattedDecks();
        var formattedRingside = formattedCardData.Ringside;
        int selectedCardIndex = 
            _view.AskPlayerToSelectCardsToRecover(_playerData.Name, _cardsToRecover, formattedRingside);
        _playerData.DecksController.PassCardFromRingsideToTheBackOfTheArsenal(selectedCardIndex);
    }
}