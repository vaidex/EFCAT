using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class DoubleAttribute : TypeAttribute {
    public double Min { get => (double)(base.Min ?? 0); set => base.Min = value; }
    public double Max { get => (double)(base.Max ?? 0); set => base.Max = value; }

    public DoubleAttribute() : base("double", Double.MaxValue) { }
    protected DoubleAttribute(object size) : base("double", size) { }
    public DoubleAttribute(int digits, int decimals) : base("double", $"{digits + decimals},{decimals}") { }
}