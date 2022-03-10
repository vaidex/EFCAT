using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public abstract class XValidationAttribute : ValidationAttribute {
    // Returns an ValidationSuccess
    protected ValidationResult? Success => ValidationResultManager.Success();
    // Returns an ValidationError with the ErrorMessage
    protected ValidationResult? Error;
}
