
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFCAT.Model.Data.Annotation;

public class CompareAttribute : XValidationAttribute {
    Parameter? _parameter = null;

    public CompareAttribute(string propertyName) : this(EParameter.PROPERTY, propertyName) { }
    public CompareAttribute(EParameter type, string name, params object[] parameter) => _parameter = new Parameter(type, name, parameter);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage, "@displayname needs to be equal to @comparisonname.", new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@comparisonname", _parameter.Name } });
        object? result = _parameter.GetValue(context);
        if(result != null && value != null)
            if(result.Equals(value)) return Success;
        return Error;
    }
}
