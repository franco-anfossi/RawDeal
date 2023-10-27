using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicReversalCardController : CardController
{
    public BasicReversalCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        throw new EndOfTurnException();
    }
    
}