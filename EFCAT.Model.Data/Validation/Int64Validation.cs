namespace EFCAT.Model.Data.Validation;

public class Int64Validation : TypeValidation<long, long> {
    public override long GetRange(long value) => value;
}