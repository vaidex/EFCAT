
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data;

public static class ValidationResultManager {
    public static ValidationResult? Success() => ValidationResult.Success;
    public static ValidationResult? Error(ValidationContext context, string? ErrorMessage, string DefaultMessage, Dictionary<string, object>? parameter = null) {
        string errorMessage = ErrorMessage ?? DefaultMessage;
        if (parameter != null) foreach (var item in parameter) if(item.Value != null) errorMessage = errorMessage.Replace(item.Key, item.Value.ToString());
        return new ValidationResult(errorMessage, new[] { context.MemberName });
    }
}