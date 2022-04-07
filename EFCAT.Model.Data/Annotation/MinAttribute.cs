using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MinAttribute : XValidationAttribute {
    private object? _min = null;

    private Parameter? _parameter = null;

    public MinAttribute(object min) => _min = min;
    public MinAttribute(EParameter type, string name, params object[] parameter) => _parameter = new Parameter(type, name, parameter);
    public MinAttribute(Type type, string min) => _min = Convert.ChangeType(min, type);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        _min = _parameter?.GetValue(context) ?? _min;
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to have a minimum of @min.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@min", _min } }
            );
        return ObjectExtension.RangeCompare(value, _min, Expression.LessThan) ? Error : Success;
    }
}

