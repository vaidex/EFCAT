using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MaxAttribute : ValidationAttribute {
    public object Max { get; set; }

    public MaxAttribute(int max = 0) { Max = max; }
    public MaxAttribute(long max = 0) { Max = max; }
    public MaxAttribute(double max = 0) { Max = max; }
    public MaxAttribute(float max = 0) { Max = max; }

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to have a maximum of @max.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@max", $"{Max}");

    private ValidationResult Error => new ValidationResult(ErrorMessage);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if (value == null) return ValidationResult.Success;
        SetError(context);
        switch (value.GetType().Name.ToUpper()) {
            case "INT32":
                if ((int)value > (int)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "INT64":
                if ((long)value > (long)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "DOUBLE":
                if ((double)value > (double)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "DECIMAL":
                if ((decimal)value > (decimal)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "FLOAT":
                if ((float)value > (float)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "STRING":
            default:
                if ((value.ToString() ?? "").Length > (int)Max) return Error;
                break;
        }
        return ValidationResult.Success;
    }
}
