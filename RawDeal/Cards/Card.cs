using RawDealView.Formatters;

namespace RawDeal.Cards;

public class Card : IViewableCardInfo
{
    public string Title { get; protected set; }
    public List<string> Types { get; protected set; }
    public List<string> Subtypes { get; protected set; }
    public string Fortitude { get; protected set; }
    public string Damage { get; protected set; }
    public string StunValue { get; protected set; }
    public string CardEffect { get; protected set; }

    public Card(CardData cardData)
    {
        Title = cardData.Title;
        Types = cardData.Types;
        Subtypes = cardData.Subtypes;
        Fortitude = cardData.Fortitude;
        Damage = cardData.Damage;
        StunValue = cardData.StunValue;
        CardEffect = cardData.CardEffect;
    }

    public bool CompareCardTitle(string otherCardTitle)
    {
        return Title == otherCardTitle;
    }
    public object Clone()
    {
        return MemberwiseClone();
    }
}