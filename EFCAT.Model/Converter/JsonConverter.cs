using EFCAT.Model.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCAT.Model.Converter;

public sealed class JsonConverter<TObject> : ValueConverter<Json<TObject>, string> where TObject : class, new() {
    public JsonConverter() : base(value => value.ToString(), value => new Json<TObject>(value)) { }
}