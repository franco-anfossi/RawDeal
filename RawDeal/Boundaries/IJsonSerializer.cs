using System.Text.Json.Serialization;

namespace RawDeal.Boundaries;

public interface IJsonSerializer
{
    T Deserialize<T>(string json);
    string Serialize<T>(T value);
    void AddConverter(JsonConverter converter); 
}

