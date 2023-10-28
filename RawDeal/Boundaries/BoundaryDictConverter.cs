using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RawDeal.Boundaries;

public class BoundaryDictConverter<TKey, TValue> : JsonConverter<BoundaryDict<TKey, TValue>> where TKey : notnull
{
    public override BoundaryDict<TKey, TValue> Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var standardDict = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);
        
        return new BoundaryDict<TKey, TValue>(standardDict);
    }

    public override void Write(Utf8JsonWriter writer, BoundaryDict<TKey, TValue> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToDictionary(), options); 
    }
}

