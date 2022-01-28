using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class RangeAttribute : ValidationAttribute {
    public object Min { get; set; }
    public object Max { get; set; }

    public RangeAttribute(int min = 0, int max = 0) => SetRange(min, max);
    public RangeAttribute(long min = 0, long max = 0) => SetRange(min, max);
    public RangeAttribute(double min = 0, double max = 0) => SetRange(min, max);
    public RangeAttribute(float min = 0, float max = 0) => SetRange(min, max);
    private void SetRange(object min, object max) { Min = min; Max = max; }

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to be in the range of @max and @max.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@min", $"{Min}")
        .Replace("@max", $"{Max}");

    private ValidationResult Error => new ValidationResult(ErrorMessage);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if (value == null) return ValidationResult.Success;
        SetError(context);
        switch (value.GetType().Name.ToUpper()) {
            case "INT32":
                if ((int)value < (int)Convert.ChangeType(Min, value.GetType())) return Error;
                if ((int)value > (int)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "INT64":
                if ((long)value < (long)Convert.ChangeType(Min, value.GetType())) return Error;
                if ((long)value > (long)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "DOUBLE":
                if ((double)value < (double)Convert.ChangeType(Min, value.GetType())) return Error;
                if ((double)value > (double)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "DECIMAL":
                if ((decimal)value < (decimal)Convert.ChangeType(Min, value.GetType())) return Error;
                if ((decimal)value > (decimal)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "FLOAT":
                if ((float)value < (float)Convert.ChangeType(Min, value.GetType())) return Error;
                if ((float)value > (float)Convert.ChangeType(Max, value.GetType())) return Error;
                break;
            case "STRING":
            default:
                if ((value.ToString() ?? "").Length < (int)Min) return Error;
                if ((value.ToString() ?? "").Length > (int)Max) return Error;
                break;
        }
        return ValidationResult.Success;
    }
}
