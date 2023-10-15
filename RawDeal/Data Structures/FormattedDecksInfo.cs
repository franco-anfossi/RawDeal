namespace RawDeal.Data_Structures;

public class FormattedDecksInfo
{
    public readonly List<string> Hand;
    public readonly List<string> Arsenal;
    public readonly List<string> Ringside;
    public readonly List<string> RingArea;
    
    public FormattedDecksInfo(DecksInfo decksInfo)
    {
        Hand = Utils.FormatDecksOfCards(decksInfo.Hand);
        Arsenal = Utils.FormatDecksOfCards(decksInfo.Arsenal);
        Ringside = Utils.FormatDecksOfCards(decksInfo.Ringside);
        RingArea = Utils.FormatDecksOfCards(decksInfo.RingArea);
    }
}