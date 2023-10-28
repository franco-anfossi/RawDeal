using RawDeal.Boundaries;
using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class DecksInfo
{
    public readonly BoundaryList<IViewableCardInfo> Hand;
    public readonly BoundaryList<IViewableCardInfo> Arsenal;
    public readonly BoundaryList<IViewableCardInfo> Ringside;
    public readonly BoundaryList<IViewableCardInfo> RingArea;
    
    public DecksInfo(BoundaryList<IViewableCardInfo> arsenal)
    {
        Hand = new BoundaryList<IViewableCardInfo>();
        Arsenal = arsenal;
        Ringside = new BoundaryList<IViewableCardInfo>();
        RingArea = new BoundaryList<IViewableCardInfo>();
    }
}