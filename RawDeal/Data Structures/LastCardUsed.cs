using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class LastCardUsed
{
    public IViewableCardInfo LastCard { get; set; }
    public string CardDamage { get; set; }
    public string CardFortitude { get; set; }
    public string UsedType { get; set; }
    
    public LastCardUsed(IViewablePlayInfo lastPlay)
    {
        LastCard = lastPlay.CardInfo;
        CardDamage = lastPlay.CardInfo.Damage;
        CardFortitude = lastPlay.CardInfo.Fortitude;
        UsedType = lastPlay.PlayedAs;
    }
    
    
    
}