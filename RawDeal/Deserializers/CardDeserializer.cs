using System.Text.Json;
using RawDeal.Cards;

namespace RawDeal.Deserializers;

public class CardDeserializer
{
    private List<Card>? _cards;
    private List<CardData>? _deserializedCards;
    private static string _cardsJsonPath = Path.Combine("data", "cards.json");
    
    public List<Card> DeserializeCards()
    {
        string jsonCardsArchive = File.ReadAllText(_cardsJsonPath);
        _deserializedCards = JsonSerializer.Deserialize<List<CardData>>(jsonCardsArchive)!;
        _cards = _deserializedCards.Select(cardData => new Card(cardData)).ToList();

        return _cards ?? throw new InvalidOperationException();
    }
}