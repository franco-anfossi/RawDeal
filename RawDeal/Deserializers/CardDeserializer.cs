using System.Text.Json;

namespace RawDeal.Deserializers;

public class CardDeserializer
{
    private List<Card>? _deserializedCards;
    private static string _cardsJsonPath = Path.Combine("data", "cards.json");
    
    public List<Card> DeserializeCards()
    {
        string jsonCardsArchive = File.ReadAllText(_cardsJsonPath);
        _deserializedCards = JsonSerializer.Deserialize<List<Card>>(jsonCardsArchive);
        return _deserializedCards ?? throw new InvalidOperationException();
    }
}