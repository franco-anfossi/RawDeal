using System.Text.Json;
using RawDeal.Superstars;
using RawDealView.Formatters;

namespace RawDeal;

public static class Utils
{
    private static string? _logoValue;
    private static string? _jsonCardsArchive;
    private static string? _jsonSuperstarsArchive;
    private static List<Card>? _deserializedCards;
    private static List<Dictionary<string, JsonElement>>? _deserializedSuperstars;
    
    public static string[] OpenDeckArchive(string archive)
    {
        string[] archiveLines = File.ReadAllLines(archive);
        return archiveLines;
    }
    public static void ChangePositionsOfTheList<T>(List<T> list)
    {
        (list[0], list[1]) = (list[1], list[0]);
    }

    public static List<string> FormatDecksOfCards(List<IViewableCardInfo> deckOfCards)
    {
        List<string> formattedCardData = new List<string>();
        foreach (var card in deckOfCards)
        {
            string formattedCard = Formatter.CardToString(card);
            formattedCardData.Add(formattedCard);
        }

        return formattedCardData;
    }
    
    public static (List<Player>, List<Card>) DeserializeCardsAndSuperstars()
    {
        string cardsJsonPath = Path.Combine("data", "cards.json");
        string superstarsJsonPath = Path.Combine("data", "superstar.json");
        
        _jsonSuperstarsArchive = File.ReadAllText(superstarsJsonPath);
        var deserializedSuperstars = DeserializeSuperstars();
        
        _jsonCardsArchive = File.ReadAllText(cardsJsonPath);
        var deserializedCards = DeserializeCards();

        return (deserializedSuperstars, deserializedCards)!;
    }
    private static List<Card> DeserializeCards()
    {
        _deserializedCards = JsonSerializer.Deserialize<List<Card>>(_jsonCardsArchive);
        return _deserializedCards ?? throw new InvalidOperationException();
    }
    
    private static List<Player?> DeserializeSuperstars()
    {
        _deserializedSuperstars = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(_jsonSuperstarsArchive);
        List<Player?> superstars = AddSuperstarAfterDeserialize();
        
        return superstars;
    }
    private static List<Player?> AddSuperstarAfterDeserialize()
    {
        List<Player?> superstars = new List<Player?>();
        foreach (var item in _deserializedSuperstars!)
        {
            _logoValue = item["Logo"].GetString();
            _jsonSuperstarsArchive = JsonSerializer.Serialize(item);
            Player superstar = CreateSuperstarClass();
            superstars.Add(superstar);
        }
        return superstars;
    }
    private static Player CreateSuperstarClass()
    {
        SuperstarData superstarData = JsonSerializer.Deserialize<SuperstarData>(_jsonSuperstarsArchive);
        
        Player superstar = _logoValue switch
        {
            "StoneCold" => new StoneCold(superstarData),
            "Undertaker" => new Undertaker(superstarData),
            "HHH" => new HHH(superstarData),
            "Jericho" => new Jericho(superstarData),
            "Mankind" => new Mankind(superstarData),
            "TheRock" => new TheRock(superstarData),
            "Kane" => new Kane(superstarData),
            _ => throw new ArgumentOutOfRangeException()
        };
        return superstar;
    }
}