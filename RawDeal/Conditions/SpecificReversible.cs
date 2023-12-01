using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class SpecificReversible : Condition
{
    private readonly string _reversalPlayTitle;
    
    public SpecificReversible(IViewablePlayInfo selectedPlay, string reversalPlayTitle) : base(selectedPlay)
        => _reversalPlayTitle = reversalPlayTitle;

    public override bool Check()
        => SelectedPlay.CardInfo.Title == _reversalPlayTitle;
}