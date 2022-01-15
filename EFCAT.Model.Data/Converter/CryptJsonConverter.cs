using System.Buffers;
using System.Buffers.Text;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFCAT.Model.Data.Converter;

public class CryptJsonConverter<TAlgorithm> : JsonConverter<Crypt<TAlgorithm>> where TAlgorithm : IAlgorithm, new() {
    public override Crypt<TAlgorithm>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string? value = null;
        if (reader.TokenType == JsonTokenType.String) value = reader.GetString();
        return (value != null ? new Crypt<TAlgorithm>(value, false) : null);
    }

    public override void Write(Utf8JsonWriter writer, Crypt<TAlgorithm> value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.Encrypt());
    }
}

public class CryptJsonFactory : JsonConverterFactory {
    public override bool CanConvert(Type typeToConvert)  => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Crypt<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        if (!(typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Crypt<>))) throw new Exception("Invalid");

        Type elementType = typeToConvert.GetGenericArguments()[0];
        JsonConverter converter = (JsonConverter)Activator.CreateInstance(typeof(CryptJsonConverter<>).MakeGenericType(new Type[] { elementType }), BindingFlags.Instance | BindingFlags.Public, binder: null, args: null, culture: null)!;

        return converter;
    }
}