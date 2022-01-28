
namespace EFCAT.Model.Data.Extension;

internal static class TypeExtension {
    private static readonly HashSet<Type> NumericTypes = new HashSet<Type>  {
        typeof(int),  typeof(double),  typeof(decimal),
        typeof(long), typeof(short),   typeof(sbyte),
        typeof(byte), typeof(ulong),   typeof(ushort),
        typeof(uint), typeof(float)
    };

    internal static bool IsNumeric(this Type myType) {
        return NumericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
    }
}
