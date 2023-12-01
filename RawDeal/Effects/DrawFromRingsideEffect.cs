using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class DrawFromRingsideEffect : Effect
{
    private readonly int _cardsToDraw;
    
    public DrawFromRingsideEffect(ImportantPlayerData superstarData, 
        View view, int cardsToDraw) : base(superstarData, view)
    {
        _cardsToDraw = cardsToDraw;
    }
    
    public override void Apply()
    {
        for (int numCardsToDraw = _cardsToDraw; numCardsToDraw > 0; numCardsToDraw--)
            DrawCard(numCardsToDraw);
    }

    private void DrawCard(int numCardsToDraw)
    {
        var formattedCardData = PlayerData.DecksController.BuildFormattedDecks();
        var formattedRingside = formattedCardData.Ringside;
        int selectedCardIndex = View.AskPlayerToSelectCardsToPutInHisHand(
            PlayerData.Name, numCardsToDraw, formattedRingside.ToList());
        PlayerData.DecksController.PassCardFromRingsideToHand(selectedCardIndex);
    }
}