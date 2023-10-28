using RawDeal.Boundaries;

namespace RawDeal.Data_Structures;

public class FormattedDecksInfo
{
    public readonly BoundaryList<string> Hand;
    public readonly BoundaryList<string> Arsenal;
    public readonly BoundaryList<string> Ringside;
    public readonly BoundaryList<string> RingArea;
    
    public FormattedDecksInfo(DecksInfo decksInfo)
    {
        Hand = Utils.FormatDecksOfCards(decksInfo.Hand);
        Arsenal = Utils.FormatDecksOfCards(decksInfo.Arsenal);
        Ringside = Utils.FormatDecksOfCards(decksInfo.Ringside);
        RingArea = Utils.FormatDecksOfCards(decksInfo.RingArea);
    }
}