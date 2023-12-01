using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;

namespace RawDeal.Effects;

public class BasicReversalEffect : Effect
{
    public BasicReversalEffect(ImportantPlayerData superstarData, View view) : base(superstarData, view) { }
    
    public override void Apply()
        => throw new EndOfTurnException("End of turn");
}