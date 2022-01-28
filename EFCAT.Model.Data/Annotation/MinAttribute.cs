using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class MinAttribute : ValidationAttribute {
    public object Min { get; set; }

    public MinAttribute(int min = 0) { Min = min; }
    public MinAttribute(long min = 0) { Min = min; }
    public MinAttribute(double min = 0) { Min = min; }
    public MinAttribute(float min = 0) { Min = min; }

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to have a minimum of @min.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{Min}");

    private ValidationResult Error => new ValidationResult(ErrorMessage);
    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if (value == null) return ValidationResult.Success;
        SetError(context);
        switch (value.GetType().Name.ToUpper()) {
            case "INT32":
                if ((int)value < (int)Convert.ChangeType(Min, value.GetType())) return Error;
                break;
            case "INT64":
                if ((long)value < (long)Convert.ChangeType(Min, value.GetType())) return Error;
                break;
            case "DOUBLE":
                if ((double)value < (double)Convert.ChangeType(Min, value.GetType())) return Error;
                break;
            case "DECIMAL":
                if ((decimal)value < (decimal)Convert.ChangeType(Min, value.GetType())) return Error;
                break;
            case "FLOAT":
                if ((float)value < (float)Convert.ChangeType(Min, value.GetType())) return Error;
                break;
            case "STRING":
            default:
                if ((value.ToString() ?? "").Length < (int)Min) return Error;
                break;
        }
        return ValidationResult.Success;
    }
}
