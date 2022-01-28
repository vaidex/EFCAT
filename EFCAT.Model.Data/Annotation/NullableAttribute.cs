using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class NullableAttribute : ValidationAttribute {
    public bool Nullable { get; set; }
    public NullableAttribute(bool nullable = true) { Nullable = nullable; }

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname must have a value.")
        .Replace("@displayname", context.DisplayName);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        return String.IsNullOrWhiteSpace(value?.ToString() ?? "") ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
}
