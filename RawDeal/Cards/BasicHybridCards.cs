namespace RawDeal.Cards;

public class BasicHybridCards : Card
{
    public BasicHybridCards(CardData cardData) : base(cardData)
    {
        Title = cardData.Title;
        Types = cardData.Types;
        Subtypes = cardData.Subtypes;
        Fortitude = cardData.Fortitude;
        Damage = cardData.Damage;
        StunValue = cardData.StunValue;
        CardEffect = cardData.CardEffect;
    }
}