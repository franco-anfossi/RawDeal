using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class IsJockeyingForPosition : Condition
{
    public IsJockeyingForPosition(IViewablePlayInfo selectedPlay) : base(selectedPlay)
    {
        SelectedPlay = selectedPlay;
    }
    
    public override bool Check()
    {
        return SelectedPlay.CardInfo.Title == "Jockeying for Position";
    }
}