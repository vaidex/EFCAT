using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class FloatAttribute : TypeAttribute {
    public float Min { get => (float)(base.Min ?? 0); set => base.Min = value; }
    public float Max { get => (float)(base.Max ?? 0); set => base.Max = value; }
    public FloatAttribute() : base("float", Decimal.MaxValue) { }
    protected FloatAttribute(object size) : base("float", size) { }
    public FloatAttribute(int digits, int decimals) : base("float", $"{digits + decimals},{decimals}") { }
}