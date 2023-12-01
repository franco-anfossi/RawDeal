using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class AskToDrawEffect : Effect
{
    private readonly int _maxCardsToDraw;
    
    public AskToDrawEffect(ImportantPlayerData superstarData, View view, int maxCardsToDraw) : base(superstarData, view)
        => _maxCardsToDraw = maxCardsToDraw;

    public override void Apply()
    {
        var cardsToDraw = View.AskHowManyCardsToDrawBecauseOfACardEffect(PlayerData.Name, _maxCardsToDraw);
        var drawCardsEffect = new DrawCardsEffect(PlayerData, View, cardsToDraw);
        drawCardsEffect.Apply();
    }
}