using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class PatternAttribute : ValidationAttribute {
    private string _pattern;

    public PatternAttribute(string pattern) => _pattern = pattern;

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to follow the template @pattern.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@pattern", $"{_pattern}");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        return ObjectExtension.MatchPattern(value, _pattern) ? Error : Success;
    }

    
}
