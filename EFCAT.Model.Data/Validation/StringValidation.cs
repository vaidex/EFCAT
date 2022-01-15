namespace EFCAT.Model.Data.Validation;

public class StringValidation : TypeValidation<string, long> {
    public override long GetRange(string value) => value.Length;
}