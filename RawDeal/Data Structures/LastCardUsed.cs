using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class LastCardUsed
{
    public string CardDamage { get; set; }
    public string CardFortitude { get; set; }
    public string UsedType { get; set; }
    
    public LastCardUsed(string damage, string fortitude, string playedAs)
    {
        CardDamage = damage;
        CardFortitude = fortitude;
        UsedType = playedAs;
    }
}