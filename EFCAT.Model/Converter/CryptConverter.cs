using EFCAT.Model.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCAT.Model.Converter;

public sealed class CryptConverter<TAlgorithm> : ValueConverter<Crypt<TAlgorithm>, string> where TAlgorithm : IAlgorithm, new() {
    public CryptConverter() : base(value => value.ToString(), value => new Crypt<TAlgorithm>(value)) { }
}