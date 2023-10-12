using System.Text.Json;
using RawDeal.Data_Structures;
using RawDeal.Superstars;

namespace RawDeal.Deserializers;

public class SuperstarDeserializer
{
    private string? _logoValue;
    private Player? _superstar;
    private SuperstarData? _superstarData;
    private readonly List<Player>? _allSuperstars = new();
    private readonly string _jsonSuperstars = File.ReadAllText(_superstarsJsonPath); 
    private List<Dictionary<string, JsonElement>>? _deserializedSuperstars;
    private static string _superstarsJsonPath = Path.Combine("data", "superstar.json");
    
    public List<Player> DeserializeSuperstars()
    {
        _deserializedSuperstars = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(_jsonSuperstars);
        DeserializeEachSuperstar();
        return _allSuperstars ?? throw new InvalidOperationException();
    }

    private void DeserializeEachSuperstar()
    {
        foreach (var item in _deserializedSuperstars!)
        {
            _logoValue = item["Logo"].GetString();
            string jsonSuperstars = JsonSerializer.Serialize(item);
            _superstarData = JsonSerializer.Deserialize<SuperstarData>(jsonSuperstars);
            InitializeSuperstarClass();
            _allSuperstars!.Add(_superstar!);
        }
    }

    private void InitializeSuperstarClass()
    {
        _superstar = _logoValue switch
        {
            "StoneCold" => new StoneCold(_superstarData!),
            "Undertaker" => new Undertaker(_superstarData!),
            "HHH" => new HHH(_superstarData!),
            "Jericho" => new Jericho(_superstarData!),
            "Mankind" => new Mankind(_superstarData!),
            "TheRock" => new TheRock(_superstarData!),
            "Kane" => new Kane(_superstarData!),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}