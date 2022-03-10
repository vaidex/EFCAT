
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class CompareAttribute : ValidationAttribute {
    public string PropertyName { get; set; }

    public CompareAttribute(string propertyName) {
        PropertyName = propertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        object? result = context.ObjectType.GetProperty(PropertyName)?.GetValue(context.ObjectInstance, null);
        if(result != null && value != null)
            if(result.Equals(value)) return ValidationResult.Success;
        return new ValidationResult(ErrorMessage);
    }
}
