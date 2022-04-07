
namespace EFCAT.Model.Data.Extension;

internal static class TypeExtension {
    private static readonly HashSet<Type> NumericTypes = new HashSet<Type>  {
        typeof(int),  typeof(double),  typeof(decimal),
        typeof(long), typeof(short),   typeof(sbyte),
        typeof(byte), typeof(ulong),   typeof(ushort),
        typeof(uint), typeof(float)
    };

    internal static bool IsNumeric(this Type type) => NumericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);

    private static readonly HashSet<Type> DateTypes = new HashSet<Type>  {
        typeof(DateOnly), typeof(DateTime), 
        typeof(TimeSpan), typeof(TimeOnly)
    };

    internal static bool IsDate(this Type type) => DateTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
}
