using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Data.Annotation;

public class TypeAttribute : XValidationAttribute {
    public string Type { get; private set; }
    protected object? Min { get; set; }
    protected object? Max { get; set; }
    public string? Pattern { get; set; }
    public bool? Nullable { get; set; }

    public TypeAttribute(string type, object? size = null) => Type = size == null ? type : $"{type}({size})";

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage,
            $"The field @displayname needs to be in the range of @min and @max{ ((Pattern != null) ? $" and match with the pattern @pattern" : "") }.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@min", Min }, { "@max", Max }, { "@pattern", Pattern } }
            );

        if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            if (Nullable != null)
                if ((bool)Nullable) return Success;
                else return Error;
            else return Success;
        if (Min.IfNotNull(min => ObjectExtension.RangeCompare(value, min, Expression.LessThan))) return Error;
        if (Max.IfNotNull(max => ObjectExtension.RangeCompare(value, max, Expression.GreaterThan))) return Error;
        if (Pattern.IfNotNull(pattern => ObjectExtension.MatchPattern(value, new Regex(pattern)))) return Error;
        return Success;
    }

    public void MaxSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has max size of {size}");
    public void MinSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has min size of {size}");
}