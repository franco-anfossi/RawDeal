using RawDealView.Formatters;

namespace RawDeal.Conditions;

public abstract class Condition
{
    protected readonly IViewablePlayInfo SelectedPlay;

    protected Condition(IViewablePlayInfo selectedPlay)
        => SelectedPlay = selectedPlay;
    
    public abstract bool Check();
}