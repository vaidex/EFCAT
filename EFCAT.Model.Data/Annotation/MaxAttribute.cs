using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MaxAttribute : XValidationAttribute {
    private object _max;

    public MaxAttribute(int max) => SetMax(max);
    public MaxAttribute(long max) => SetMax(max);
    public MaxAttribute(double max) => SetMax(max);
    public MaxAttribute(float max) => SetMax(max);
    private void SetMax(object max) => _max = max;

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to have a maximum of @max.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@max", _max } }
            );
        return ObjectExtension.RangeCompare(value, _max, ErrorMessage, Expression.GreaterThan) ? Error : Success;
    }
}
