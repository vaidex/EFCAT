using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class RangeAttribute : XValidationAttribute {
    private object _min = null;
    private object _max = null;

    private Parameter? _minParameter = null;
    private Parameter? _maxParameter = null;

    public RangeAttribute(object min, object max) {
        _min = min;
        _max = max;
    }
    public RangeAttribute(EParameter minType, string minName, object[]? minParameter, EParameter maxType, string maxName, object[]? maxParameter) {
        _minParameter = new Parameter(minType, minName, minParameter);
        _maxParameter = new Parameter(maxType, maxName, maxParameter);
    }
    public RangeAttribute(Type minType, string min, Type maxType, string max) {
        _min = Convert.ChangeType(min, minType);
        _max = Convert.ChangeType(max, maxType);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        _min = _minParameter?.GetValue(context) ?? _min;
        _max = _maxParameter?.GetValue(context) ?? _max;
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to be in the range of @min and @max.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@min", _min }, { "@max", _max } }
            );
        return ObjectExtension.RangeCompare(value, _min, Expression.LessThan) || ObjectExtension.RangeCompare(value, _max, Expression.GreaterThan) ? Error : Success;
    }
}
