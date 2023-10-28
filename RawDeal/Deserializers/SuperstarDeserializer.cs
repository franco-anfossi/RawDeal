using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Superstars;

namespace RawDeal.Deserializers;

public class SuperstarDeserializer
{
    private readonly IJsonSerializer _jsonSerializer = new JsonSerializerBoundary();
    private static readonly string SuperstarsJsonPath = Path.Combine("data", "superstar.json");
    
    public SuperstarDeserializer()
    {
        AddRequiredConverters();
    }

    private void AddRequiredConverters()
    {
        _jsonSerializer.AddConverter(new BoundaryListConverter<BoundaryDict<string, object>>());
        _jsonSerializer.AddConverter(new BoundaryDictConverter<string, object>());
    }

    public BoundaryList<Player> DeserializeSuperstars()
    {
        var jsonSuperstars = File.ReadAllText(SuperstarsJsonPath);
        var deserializedSuperstars = 
            _jsonSerializer.Deserialize<BoundaryList<BoundaryDict<string, object>>>(jsonSuperstars);
        return ConvertToSuperstars(deserializedSuperstars);
    }

    private BoundaryList<Player> ConvertToSuperstars(BoundaryList<BoundaryDict<string, object>> deserializedSuperstars)
    {
        var allSuperstars = new BoundaryList<Player>();

        foreach (var item in deserializedSuperstars)
        {
            var logoValue = item["Logo"].ToString();
            var superstarData = ExtractSuperstarDataFromItem(item);
            var superstar = CreateSuperstarInstance(logoValue, superstarData);
            allSuperstars.Add(superstar);
        }

        return allSuperstars;
    }

    private SuperstarData ExtractSuperstarDataFromItem(BoundaryDict<string, object> item)
    {
        string jsonItem = _jsonSerializer.Serialize(item);
        return _jsonSerializer.Deserialize<SuperstarData>(jsonItem);
    }

    private Player CreateSuperstarInstance(string? logoValue, SuperstarData superstarData)
    {
        return logoValue switch
        {
            "StoneCold" => new StoneCold(superstarData),
            "Undertaker" => new Undertaker(superstarData),
            "HHH" => new Hhh(superstarData),
            "Jericho" => new Jericho(superstarData),
            "Mankind" => new Mankind(superstarData),
            "TheRock" => new TheRock(superstarData),
            "Kane" => new Kane(superstarData),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

