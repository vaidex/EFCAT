using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EFCAT.Model.Data.Extension;

namespace EFCAT.Model.Data.Annotation {
    public class StringAttribute : ValidationAttribute {
        public int? Min { get; set; }
        public int? Max { get; set; }
        public Regex? Pattern { get; set; }

        private ValidationResult? Error => new ValidationResult(ErrorMessage);
        private ValidationResult? Success => ValidationResult.Success;

        private void SetError(ValidationContext context) => 
            ErrorMessage = (ErrorMessage ?? $"The field {context.DisplayName} needs to have between {Min} and {Max} characters{ ((Pattern != null) ? $" and match with pattern {Pattern}" : "") }.")
            .Replace("@min", $"{Min}")
            .Replace("@max", $"{Max}")
            .Replace("@pattern", $"{Pattern}");

        protected override ValidationResult? IsValid(object? value, ValidationContext context) {
            string? _value = Convert.ToString(value);
            int? length = _value != null ? _value.Length : null;
            SetError(context);
            if (_value == null) return ValidationResult.Success;
            else if (Min.IfNotNull(min => length < min)) return Error;
            else if (Max.IfNotNull(max => length > max)) return Error;
            else if (Pattern.IfNotNull(pattern => pattern.IsMatch(_value))) return Error;
            return Success;
        }
    }

    public class BaseValidationAttribute : ValidationAttribute {

    }

    public class TypeAttribute<TType> where TType : struct {
        public TType? Min { get; set; }
        public TType? Max { get; set; }
        public Regex? Pattern { get; set; }

        public string? ErrorMessage { get; set; }

        private ValidationResult? Error => new ValidationResult(ErrorMessage);
        private ValidationResult? Success => ValidationResult.Success;

        private void SetError(ValidationContext context) =>
            ErrorMessage = (ErrorMessage ?? $"The field {context.DisplayName} needs to have between {Min} and {Max} characters{ ((Pattern != null) ? $" and match with pattern {Pattern}" : "") }.")
            .Replace("@min", $"{Min}")
            .Replace("@max", $"{Max}")
            .Replace("@pattern", $"{Pattern}");
    }
}
