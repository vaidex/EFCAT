using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MaxAttribute : ValidationAttribute {
    private object _max;

    public MaxAttribute(int max) => SetMax(max);
    public MaxAttribute(long max) => SetMax(max);
    public MaxAttribute(double max) => SetMax(max);
    public MaxAttribute(float max) => SetMax(max);
    private void SetMax(object max) => _max = max;

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to have a maximum of @max.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@max", $"{_max}");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        return ObjectExtension.RangeCompare(value, _max, ErrorMessage, Expression.GreaterThan) ? Error : Success;
    }
}
