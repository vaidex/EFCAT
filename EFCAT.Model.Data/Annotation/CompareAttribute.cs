
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFCAT.Model.Data.Annotation;

public class CompareAttribute : XValidationAttribute {
    public string PropertyName { get; set; }

    public CompareAttribute(string propertyName) {
        PropertyName = propertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        PropertyInfo? property = context.ObjectType.GetProperty(PropertyName);
        if (property == null) throw new Exception($"{context.DisplayName} comparison property not found");
        Error = ValidationResultManager.Error(context, ErrorMessage, "@displayname needs to be equal to @comparisonname.", new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@comparisonname", property.Name } });
        object? result = property.GetValue(context.ObjectInstance, null);
        if(result != null && value != null)
            if(result.Equals(value)) return Success;
        return Error;
    }
}
