using System.Text.Json;
using RawDeal.Data_Structures;

namespace RawDeal.Deserializers;

public class CardDeserializer
{
    private List<CardData>? _deserializedCards;
    private static string _cardsJsonPath = Path.Combine("data", "cards.json");
    
    public List<CardData> DeserializeCards()
    {
        string jsonCardsArchive = File.ReadAllText(_cardsJsonPath);
        _deserializedCards = JsonSerializer.Deserialize<List<CardData>>(jsonCardsArchive)!;

        return _deserializedCards ?? throw new InvalidOperationException();
    }
}