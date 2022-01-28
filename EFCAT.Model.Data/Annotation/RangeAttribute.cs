using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class RangeAttribute : ValidationAttribute {
    private object _min;
    private object _max;

    public RangeAttribute(int min, int max) => SetRange(min, max);
    public RangeAttribute(long min, long max) => SetRange(min, max);
    public RangeAttribute(double min, double max) => SetRange(min, max);
    public RangeAttribute(float min, float max) => SetRange(min, max);
    private void SetRange(object min, object max) { _min = min; _max = max; }

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to be in the range of @max and @max.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{_min}")
        .Replace("@max", $"{_max}");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        return ObjectExtension.RangeCompare(value, _min, ErrorMessage, Expression.LessThan) || ObjectExtension.RangeCompare(value, _max, ErrorMessage, Expression.GreaterThan) ? Error : Success;
    }
}
