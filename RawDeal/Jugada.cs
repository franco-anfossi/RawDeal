using RawDealView.Formatters;

namespace RawDeal;

public class Jugada : IViewablePlayInfo
{
    public IViewableCardInfo CardInfo { get; }
    public string PlayedAs { get; }
    
    public Jugada(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }
}