using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class MinAttribute : XValidationAttribute {
    private object _min;

    public MinAttribute(int min) => SetMin(min);
    public MinAttribute(long min) => SetMin(min);
    public MinAttribute(double min) => SetMin(min);
    public MinAttribute(float min) => SetMin(min);
    private void SetMin(object min) => _min = min;

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to have a minimum of @max.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@min", _min } }
            );
        return ObjectExtension.RangeCompare(value, _min, Expression.LessThan) ? Error : Success;
    }
}
