using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class RangeAttribute : XValidationAttribute {
    private object _min;
    private object _max;

    public RangeAttribute(int min, int max) => SetRange(min, max);
    public RangeAttribute(long min, long max) => SetRange(min, max);
    public RangeAttribute(double min, double max) => SetRange(min, max);
    public RangeAttribute(float min, float max) => SetRange(min, max);
    private void SetRange(object min, object max) { _min = min; _max = max; }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to be in the range of @min and @max.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@min", _min }, { "@max", _max } }
            );
        return ObjectExtension.RangeCompare(value, _min, Expression.LessThan) || ObjectExtension.RangeCompare(value, _max, Expression.GreaterThan) ? Error : Success;
    }
}
