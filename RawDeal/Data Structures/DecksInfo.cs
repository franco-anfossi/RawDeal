using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class DecksInfo
{
    public List<IViewableCardInfo> Hand;
    public List<IViewableCardInfo> Arsenal;
    public List<IViewableCardInfo> Ringside;
    public List<IViewableCardInfo> RingArea;
    
    public DecksInfo(List<IViewableCardInfo> arsenal)
    {
        Hand = new List<IViewableCardInfo>();
        Arsenal = arsenal;
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
    }
}