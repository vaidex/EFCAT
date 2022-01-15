using System.ComponentModel.DataAnnotations;
using EFCAT.Model.Data.Validation;

namespace EFCAT.Model.Data.Annotation;

public class DoubleAttribute : TypeAttribute {
    private DoubleValidation validation = new DoubleValidation();

    public double Min { get => validation.Min; set => validation.Min = value; }
    public double Max { get => validation.Max; set => validation.Max = value; }
    public override bool Nullable { get => validation.Nullable; set => validation.Nullable = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public DoubleAttribute() : base("double", Double.MaxValue) { }
    public DoubleAttribute(string type, object size) : base(type, size) { Max = (double)size; }
    public DoubleAttribute(string type, int digits, int decimals) : base(type, $"{digits + decimals},{decimals}") { Max = ((Math.Pow(10, digits) - 1) + ((Math.Pow(10, decimals) - 1) / Math.Pow(10, decimals))); }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}