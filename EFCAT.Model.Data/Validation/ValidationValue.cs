
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Validation;

public class ValidationValue<T> {
    public T Value { get; set; }
    public string? ErrorMessage { get; set; }

    public static implicit operator ValidationValue<T>(T value) => new ValidationValue<T>() { Value = value };
    public static implicit operator ValidationValue<T>(object[] values) => new ValidationValue<T>() { Value = (T)values[0], ErrorMessage = (string)values[1] };

    public static explicit operator T(ValidationValue<T> value) => value.Value;

    public ValidationResult? GetError(ValidationContext context) => new ValidationResult((ErrorMessage ?? $"Please set an Error Message for field @displayname.")
        .Replace("@displayname", context.DisplayName)
        .Replace("@value", $"{Value}"));
}