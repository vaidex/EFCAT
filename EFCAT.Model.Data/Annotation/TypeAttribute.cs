using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Data.Annotation;

public class TypeAttribute : ValidationAttribute {
    public string Type { get; private set; }
    protected object? Min { get; set; }
    protected object? Max { get; set; }
    protected Regex? Pattern { get; set; }
    public bool? Nullable { get; set; }

    public TypeAttribute(string type, object? size = null) => Type = size == null ? type : $"{type}({size})";

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to be in the range of @min and @max{ ((Pattern != null) ? $" and match with the pattern @pattern" : "") }.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{Min}")
        .Replace("@max", $"{Max}")
        .Replace("@pattern", $"{Pattern}");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Console.WriteLine(context.DisplayName + " > " + (Nullable == null) + " : " + (Min == null) + " : " + (Max == null) + " : " + (Pattern == null));
        SetError(context);
        if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            if (Nullable != null)
                if ((bool)Nullable) return Success;
                else return Error;
            else return Success;
        if (Min.IfNotNull(min => ObjectExtension.RangeCompare(value, min, ErrorMessage, Expression.LessThan))) return Error;
        if (Max.IfNotNull(max => ObjectExtension.RangeCompare(value, max, ErrorMessage, Expression.GreaterThan))) return Error;
        if (Pattern.IfNotNull(pattern => pattern.IsMatch(Convert.ToString(value)))) return Error;
        return Success;
    }

    public void MaxSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has max size of {size}");
    public void MinSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has min size of {size}");
}