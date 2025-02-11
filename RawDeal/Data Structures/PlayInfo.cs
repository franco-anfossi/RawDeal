using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class PlayInfo : IViewablePlayInfo
{
    public IViewableCardInfo CardInfo { get; }
    public string PlayedAs { get; }
    
    public PlayInfo(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }
}