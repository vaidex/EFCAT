using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MinAttribute : ValidationAttribute {
    private object _min;

    public MinAttribute(int min) => SetMin(min);
    public MinAttribute(long min) => SetMin(min);
    public MinAttribute(double min) => SetMin(min);
    public MinAttribute(float min) => SetMin(min);
    private void SetMin(object min) => _min = min;

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to have a minimum of @min.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{_min}");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        return ObjectExtension.RangeCompare(value, _min, ErrorMessage, Expression.LessThan) ? Error : Success;
    }

    
}
