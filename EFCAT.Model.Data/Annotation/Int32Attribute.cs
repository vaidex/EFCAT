using System.ComponentModel.DataAnnotations;
using EFCAT.Model.Data.Validation;

namespace EFCAT.Model.Data.Annotation;

public class Int32Attribute : TypeAttribute {
    private Int32Validation validation = new Int32Validation();

    public int Min { get => validation.Min; set => validation.Min = value; }
    public int Max { get => validation.Max; set => validation.Max = value; }
    public override bool Nullable { get => validation.Nullable; set => validation.Nullable = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public Int32Attribute() : base("int", Int32.MaxValue) { }
    public Int32Attribute(string type, object size) : base(type, size) { Max = (int)size; }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}