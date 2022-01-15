using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EFCAT.Model.Data.Extension;

namespace EFCAT.Model.Data.Validation;

public abstract class TypeValidation<TType, TRange> where TRange : IComparable {
    public TRange? Min { get; set; }
    public TRange? Max { get; set; }
    public Regex? Pattern { get; set; }
    public bool Nullable { get; set; } = false;

    public string? ErrorMessage { get; set; }

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to be in the range of @min and @max{ ((Pattern != null) ? $" and match with the pattern @pattern" : "") }.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{Min}")
        .Replace("@max", $"{Max}")
        .Replace("@pattern", $"{Pattern}");

    public virtual ValidationResult? IsValid(object? value, ValidationContext context) {
        if (value == null || String.IsNullOrWhiteSpace((string)value))
            if (Nullable) return Success;
            else return Error;
        TType? _value = (TType)value;
        SetError(context);
        TRange? _range = GetRange(_value);
        if (Min.IfNotNull(min => _range.CompareTo(min) <= -1)) return Error;
        if (Max.IfNotNull(max => _range.CompareTo(max) >= 1)) return Error;
        if (Pattern.IfNotNull(pattern => pattern.IsMatch(Convert.ToString(_value)))) return Error;
        return Success;
    }

    public abstract TRange? GetRange(TType value);
}