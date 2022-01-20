
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class CompareAttribute : ValidationAttribute {
    public string PropertyName { get; set; }

    public CompareAttribute(string propertyName) {
        PropertyName = propertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        string? result = context.ObjectType.GetProperty(PropertyName)?.GetValue(context.ObjectInstance, null)?.ToString();
        if(result == (string?)value) return ValidationResult.Success;
        return new ValidationResult(ErrorMessage);
    }
}
