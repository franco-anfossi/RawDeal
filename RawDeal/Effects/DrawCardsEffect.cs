using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class DrawCardsEffect : Effect
{
    private readonly int _cardsToDraw;
    public DrawCardsEffect(ImportantPlayerData superstarData, View view, int cardsToDraw) : base(superstarData, view)
    {
        _cardsToDraw = cardsToDraw;
    }

    public override void Apply()
    {
        View.SayThatPlayerDrawCards(PlayerData.Name, _cardsToDraw);
        for (int numCardsToDraw = _cardsToDraw; numCardsToDraw > 0; numCardsToDraw--)
            PlayerData.DecksController.DrawCard();
    }
}