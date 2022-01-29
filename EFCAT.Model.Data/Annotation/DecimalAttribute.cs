using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class DecimalAttribute : TypeAttribute {
    public double Min { get => (double)(base.Min ?? 0); set => base.Min = value; }
    public double Max { get => (double)(base.Max ?? 0); set => base.Max = value; }

    public DecimalAttribute() : base("decimal", Decimal.MaxValue) { }
    protected DecimalAttribute(object size) : base("decimal", size) { }
    public DecimalAttribute(int digits, int decimals) : base("decimal", $"{digits + decimals},{decimals}") {  }
}