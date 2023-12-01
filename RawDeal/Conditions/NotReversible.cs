using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class NotReversible : Condition
{
    public NotReversible(IViewablePlayInfo selectedPlay) : base(selectedPlay) { }

    public override bool Check()
    { 
        var neverReverse = false;
        return neverReverse;
    }
}