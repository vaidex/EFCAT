using EFCAT.Model.Data.Extension;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Data.Annotation;

public class PatternAttribute : XValidationAttribute {
    private Regex? _pattern = null;

    private Parameter? _parameter = null;

    public PatternAttribute(string pattern) => _pattern = new Regex(pattern);
    public PatternAttribute(EParameter type, string name, params object[] parameter) => _parameter = new Parameter(type, name, parameter);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        if(_parameter?.GetValue(context) is object result) {
            if (result is Regex r) _pattern = r;
            else if (result is string s) _pattern = new Regex(s);
        }
        Error = ValidationResultManager.Error(context, ErrorMessage,
            "The field @displayname needs to follow the template @pattern.",
            new Dictionary<string, object> { { "@displayname", context.DisplayName }, { "@pattern", _pattern.ToString() } }
            );
        return ObjectExtension.MatchPattern(value, _pattern) ? Error : Success;
    }
}