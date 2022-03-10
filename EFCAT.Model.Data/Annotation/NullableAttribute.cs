using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class NullableAttribute : XValidationAttribute {
    public bool Nullable { get; private set; }
    public NullableAttribute(bool nullable = true) { Nullable = nullable; }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage, "The field @displayname must have a value.", new Dictionary<string, object> { { "@displayname", context.DisplayName } });
        if (value == null) return Nullable ? Success : Error;
        else return String.IsNullOrWhiteSpace(value.ToString() ?? "") ? Error : Success;
    }
}

public class NotNullAttribute : NullableAttribute {
    public NotNullAttribute(bool notnull = true) : base(!notnull) { }
}