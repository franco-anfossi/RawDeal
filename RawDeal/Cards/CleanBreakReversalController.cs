using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Exceptions;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CleanBreakReversalController : BasicCardController
{
    public CleanBreakReversalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        var discardEffect = new AskToDiscardHandCardsEffect(OpponentData, View, 4);
        discardEffect.Apply();
        
        var drawEffect = new DrawCardsEffect(PlayerData, View, 1);
        drawEffect.Apply();
        
        throw new EndOfTurnException();
    }
}