using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Extension;

public static class PropertyExtension {
    internal static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().First();

    internal static IEnumerable<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute {
        if (property == null) throw new ArgumentNullException(nameof(property));
        else if (property.Name == null) throw new ArgumentNullException(nameof(property.Name));
        else if (property.ReflectedType == null) throw new ArgumentNullException(nameof(property.ReflectedType));

        var propertyInfo = property.ReflectedType.GetProperty(property.Name);

        if (propertyInfo == null) return null;

        return propertyInfo.GetCustomAttributes<T>();
    }

    public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute {
        IEnumerable<T> attributes = GetAttributes<T>(property);
        return attributes != null && attributes.Any();
    }

    public static void OnAttribute<T>(this PropertyInfo property, Action<T> action, Action? negate = null) where T : Attribute {
        if (property.HasAttribute<T>()) action(property.GetAttribute<T>());
        else if(negate != null) negate();
    }

    public static TOut OnAttribute<T, TOut>(this PropertyInfo property, Func<T, TOut> func, Func<TOut> negate) where T : Attribute {
        if (property.HasAttribute<T>()) return func(property.GetAttribute<T>());
        return negate();
    }

    internal static string GetSqlName(this PropertyInfo property) => $"{(property.HasAttribute<ColumnAttribute>() ? property.GetAttribute<ColumnAttribute>().Name : property.Name.GetSqlName())}";
    internal static string GetSqlName(this string input) => Regex.Replace(input, "(?<=[a-z])([A-Z])", "_$1", RegexOptions.Compiled).ToUpper();

    internal static Type GetNullableType(this Type type) {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type.IsValueType) return typeof(Nullable<>).MakeGenericType(type);
        return type;
    }

    internal static Type GetPropertyType(this PropertyInfo property) => property.PropertyType.IsGenericType ? typeof(string) : property.PropertyType;
}
