using RawDealView.Formatters;

namespace RawDeal;

public class Card : IViewableCardInfo
{
    public string Title { get; private set; }
    public List<string> Types { get; private set; }
    public List<string> Subtypes { get; private set; }
    public string Fortitude { get; private set; }
    public string Damage { get; private set; }
    public string StunValue { get; private set; }
    public string CardEffect { get; private set; }

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

    public object Clone()
    {
        return MemberwiseClone();
    }
}