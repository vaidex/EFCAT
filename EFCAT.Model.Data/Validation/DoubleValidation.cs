namespace EFCAT.Model.Data.Validation;

public class DoubleValidation : TypeValidation<double, double> {
    public override double GetRange(double value) => value;
}