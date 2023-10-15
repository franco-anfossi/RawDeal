using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class DecksInfo
{
    public readonly List<IViewableCardInfo> Hand;
    public readonly List<IViewableCardInfo> Arsenal;
    public readonly List<IViewableCardInfo> Ringside;
    public readonly List<IViewableCardInfo> RingArea;
    
    public DecksInfo(List<IViewableCardInfo> arsenal)
    {
        Hand = new List<IViewableCardInfo>();
        Arsenal = arsenal;
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
    }
}