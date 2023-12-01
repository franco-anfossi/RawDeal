using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RawDeal.Boundaries;

public class BoundaryListConverter<T> : JsonConverter<BoundaryList<T>>
{
    public override BoundaryList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var standardList = JsonSerializer.Deserialize<List<T>>(ref reader, options);
        
        return new BoundaryList<T>(standardList);
    }

    public override void Write(Utf8JsonWriter writer, BoundaryList<T> value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value.ToList(), options);
}
