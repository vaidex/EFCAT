namespace EFCAT.Model.Data.Validation;

public class Int32Validation : TypeValidation<int, int> {
    public override int GetRange(int value) => value;
}