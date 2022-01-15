using System.ComponentModel.DataAnnotations;
using EFCAT.Model.Data.Validation;

namespace EFCAT.Model.Data.Annotation;

public class Int64Attribute : TypeAttribute {
    private Int64Validation validation = new Int64Validation();

    public long Min { get => validation.Min; set => validation.Min = value; }
    public long Max { get => validation.Max; set => validation.Max = value; }
    public override bool Nullable { get => validation.Nullable; set => validation.Nullable = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public Int64Attribute() : base("long", Int64.MaxValue) { }
    public Int64Attribute(string type, object size) : base(type, size) { Max = (long)size; }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}