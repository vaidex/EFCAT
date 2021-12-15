using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EFCAT.Util;

public static class Tools {
    public static void PrintInfo(string s) {
        s = $"[EFCAT] {s}";
        System.Diagnostics.Debug.WriteLine(s);
        System.Console.WriteLine(s);
    }
}

public static class ExtensionTools {

    public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().First();

    public static IEnumerable<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute {
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

    public static string GetSqlName(this PropertyInfo property) => $"{(property.HasAttribute<ColumnAttribute>() ? property.GetAttribute<ColumnAttribute>().Name : property.Name.GetSqlName())}";
    public static string GetSqlName(this string input) => Regex.Replace(input, "(?<=[a-z])([A-Z])", "_$1", RegexOptions.Compiled).ToUpper();

    public static Type GetNullableType(this Type type) {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type.IsValueType) return typeof(Nullable<>).MakeGenericType(type);
        return type;
    }
}