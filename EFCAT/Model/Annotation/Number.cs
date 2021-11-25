using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class Number : ValidationAttribute {
    public readonly double min;
    public readonly double max;
    public readonly int digits;
    public readonly int decimals;

    public Number(double min = 0, double max = 100, int digits = 3, int decimals = 2) {
        this.min = min;
        this.max = max;
        this.digits = digits;
        this.decimals = decimals;
    }

    public override bool IsValid(object? value) {
        if (value == null) throw new ArgumentNullException();
        return (double)value >= min && (double)value <= max;
    }
}

