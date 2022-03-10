using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EFCAT.Model.Data.Annotation;

public class PatternAttribute : XValidationAttribute {
    private string _pattern;

    public PatternAttribute(string pattern) => _pattern = pattern;

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to follow the template @pattern.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@pattern", _pattern } }
            );
        return ObjectExtension.MatchPattern(value, _pattern) ? Error : Success;
    }

    
}
