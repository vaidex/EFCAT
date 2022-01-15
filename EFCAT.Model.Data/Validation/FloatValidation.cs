namespace EFCAT.Model.Data.Validation;

public class FloatValidation : TypeValidation<float, float> {
    public override float GetRange(float value) => value;
}