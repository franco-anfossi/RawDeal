using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CleanBreakReversalController : CardController
{
    public CleanBreakReversalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        DiscardCards();
        DrawCards();
        
        throw new EndOfTurnException();
    }

    private void DiscardCards()
    {
        var discardEffect = new AskToDiscardHandCardsEffect(OpponentData, View, 4);
        discardEffect.Apply();
    }

    private void DrawCards()
    {
        var drawEffect = new DrawCardsEffect(PlayerData, View, 1);
        drawEffect.Apply();
    }
}