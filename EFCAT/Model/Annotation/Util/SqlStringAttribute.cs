using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Annotation.Util;

public class SqlStringAttribute : SqlLongRangeAttribute {

    public string? Pattern { get; set; } = null;

    public SqlStringAttribute(string type, int size) : base(type, size) { Max = size; if (size < 0) MinSizeReached("0"); }
    public SqlStringAttribute(string type, long size) : base(type, size) { Max = size; if (size < 0) MinSizeReached("0"); }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if (ErrorMessage == null) ErrorMessage = $"The field {context.DisplayName} needs to have between {Min} and {Max} characters{ ((Pattern != null) ? $" and match with pattern {Pattern}" : "") }.";
        ErrorMessage = ErrorMessage.Replace("@min", $"{Min}").Replace("@max", $"{Max}").Replace("@pattern", $"{Pattern}");
        if (value == null) throw new ArgumentNullException();
        return ((base.IsValid(((string)value).Length, context) == ValidationResult.Success) && (Pattern == null ? true : Regex.IsMatch((string)value, Pattern))) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
}
