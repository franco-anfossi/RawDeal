using RawDeal.Boundaries;
using RawDeal.Data_Structures;

namespace RawDeal.Deserializers;

public class CardDeserializer
{
    private readonly IJsonSerializer _jsonSerializer = new JsonSerializerBoundary();
    private static readonly string CardsJsonPath = Path.Combine("data", "cards.json");
    
    public CardDeserializer()
        => SetupJsonSerializer();

    private void SetupJsonSerializer()
        => _jsonSerializer.AddConverter(new BoundaryListConverter<CardData>());

    public BoundaryList<CardData> DeserializeCards()
    {
        string jsonCardsArchive = File.ReadAllText(CardsJsonPath);
        var deserializedCards = _jsonSerializer.Deserialize<BoundaryList<CardData>>(jsonCardsArchive);
        return deserializedCards ?? throw new InvalidOperationException();
    }
}

