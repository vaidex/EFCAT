using System.ComponentModel.DataAnnotations;
using EFCAT.Model.Data.Validation;

namespace EFCAT.Model.Data.Annotation;

public class FloatAttribute : TypeAttribute {
    private FloatValidation validation = new FloatValidation();

    public float Min { get => validation.Min; set => validation.Min = value; }
    public float Max { get => validation.Max; set => validation.Max = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public FloatAttribute() : base("decimal", Decimal.MaxValue) { }
    public FloatAttribute(string type, object size) : base(type, size) { Max = (float)size; }
    public FloatAttribute(string type, int digits, int decimals) : base(type, $"{digits + decimals},{decimals}") { Max = (float)((Math.Pow(10, digits) - 1) + ((Math.Pow(10, decimals) - 1) / Math.Pow(10, decimals))); }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}