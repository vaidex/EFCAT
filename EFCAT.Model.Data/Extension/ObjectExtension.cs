using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Data.Extension {
    internal static class ObjectExtension {
        public static bool IfNotNull<T>(this T obj, Predicate<T> predicate) {
            if (obj != null) return predicate(obj);
            return false;
        }
        internal static bool RangeCompare(object? value, object border, string? ErrorMessage, Func<Expression, Expression, BinaryExpression> CompareExpression) {
            if (value == null) return false;
            Type type = value.GetType();
            if (!type.IsNumeric()) type = typeof(int);
            value = value.GetSize();
            ParameterExpression parameter = Expression.Parameter(type, "value");
            ConstantExpression constant = Expression.Constant(Convert.ChangeType(border, type), type);
            BinaryExpression binary = CompareExpression(parameter, constant);
            Delegate compile = Expression.Lambda(binary, parameter).Compile();
            return (bool)compile.DynamicInvoke(value);
        }
        internal static object? GetSize(this object? value) {
            if (value == null) return null;
            Type type = value.GetType();
            if (type.IsNumeric()) return value;
            else if ((value.GetType().Namespace?.ToUpper() ?? "").StartsWith("SYSTEM.COLLECTIONS")) return type.GetProperty("Count")?.GetValue(value, null) ?? throw new NotImplementedException();
            else return ((value.ToString() ?? "").Length);
        }
        internal static bool MatchPattern(object? value, string pattern) =>
            (value == null) ? false : !(new Regex(pattern).IsMatch(value.ToString() ?? ""));

    }
}
