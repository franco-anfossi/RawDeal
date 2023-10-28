using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class IsJockeyingForPosition : Condition
{
    private readonly IViewablePlayInfo _selectedPlay;
    
    public IsJockeyingForPosition(IViewablePlayInfo selectedPlay)
    {
        _selectedPlay = selectedPlay;
    }
    
    public override bool Check()
    {
        return _selectedPlay.CardInfo.Title == "Jockeying for Position";
    }
}