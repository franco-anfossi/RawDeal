using System.Text.Json;
using System.Text.Json.Serialization;

namespace RawDeal.Boundaries;

public class JsonSerializerBoundary : IJsonSerializer
{
    private readonly JsonSerializerOptions _options = new();

    public T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    public void AddConverter(JsonConverter converter)
    {
        _options.Converters.Add(converter);
    }
}
