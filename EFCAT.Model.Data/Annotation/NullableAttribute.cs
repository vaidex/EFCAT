using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class NullableAttribute : ValidationAttribute {
    public bool Nullable { get; private set; }
    public NullableAttribute(bool nullable = true) { Nullable = nullable; }

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname must have a value.")
        .Replace("@displayname", context.DisplayName);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        SetError(context);
        if (value == null) return Nullable ? Success : Error;
        else return String.IsNullOrWhiteSpace(value.ToString() ?? "") ? Error : Success;
    }
}

public class NotNullAttribute : NullableAttribute {
    public NotNullAttribute(bool notnull = true) : base(!notnull) { }
}