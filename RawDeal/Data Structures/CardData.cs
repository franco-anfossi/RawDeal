using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class CardData : IViewableCardInfo
{
    public string Title { get; set; }
    public List<string> Types { get; set; }
    public List<string> Subtypes { get; set; }
    public string Fortitude { get; set; }
    public string Damage { get; set; }
    public string StunValue { get; set; }
    public string CardEffect { get; set; }
    
    public bool CompareCardTitle(string otherCardTitle)
        => Title == otherCardTitle;

    public object Clone()
        => MemberwiseClone();
}