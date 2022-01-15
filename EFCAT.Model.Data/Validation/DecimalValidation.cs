namespace EFCAT.Model.Data.Validation;

public class DecimalValidation : TypeValidation<decimal, double> {
    public override double GetRange(decimal value) => (double)value;
}