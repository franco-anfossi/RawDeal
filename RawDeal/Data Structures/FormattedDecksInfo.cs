namespace RawDeal.Data_Structures;

public class FormattedDecksInfo
{
    public List<string> Hand;
    public List<string> Arsenal;
    public List<string> Ringside;
    public List<string> RingArea;
    
    public FormattedDecksInfo(DecksInfo decksInfo)
    {
        Hand = Utils.FormatDecksOfCards(decksInfo.Hand);
        Arsenal = Utils.FormatDecksOfCards(decksInfo.Arsenal);
        Ringside = Utils.FormatDecksOfCards(decksInfo.Ringside);
        RingArea = Utils.FormatDecksOfCards(decksInfo.RingArea);
    }
}