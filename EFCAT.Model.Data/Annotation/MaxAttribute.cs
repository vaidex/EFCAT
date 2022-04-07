using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MaxAttribute : XValidationAttribute {
    private object _max;

    private Parameter? _parameter = null;

    public MaxAttribute(object min) => _max = min;
    public MaxAttribute(EParameter type, string name, params object[] parameter) => _parameter = new Parameter(type, name, parameter);
    public MaxAttribute(Type type, string min) => _max = Convert.ChangeType(min, type);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        _max = _parameter?.GetValue(context) ?? _max;
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to have a maximum of @max.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@max", _max } }
            );
        return ObjectExtension.RangeCompare(value, _max, Expression.GreaterThan) ? Error : Success;
    }
}
