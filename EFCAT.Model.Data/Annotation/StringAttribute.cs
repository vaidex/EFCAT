using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EFCAT.Model.Data.Validation;

namespace EFCAT.Model.Data.Annotation;

public class StringAttribute : TypeAttribute {
    private StringValidation validation = new StringValidation();

    public long Min { get => validation.Min; set => validation.Min = value; }
    public long Max { get => validation.Max; set => validation.Max = value; }
    public Regex Pattern { get => validation.Pattern; set => validation.Pattern = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public StringAttribute() : base("string", Int32.MaxValue) { }
    public StringAttribute(string type, object size) : base(type, size) { Max = Convert.ToInt64(size); }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}