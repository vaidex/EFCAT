using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Annotation.Util;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqlAttribute : ValidationAttribute {
    public string Type { get; private set; }
    public string Size { get; private set; }

    public SqlAttribute(string type, string size = "") {
        Type = type;
        Size = size;
    }

    public SqlAttribute(string type, int size) : this(type, $"{size}") { }

    public SqlAttribute(string type, long size) : this(type, $"{size}") { }

    public string GetTypeName() {
        string typeName = Type;
        if (Size != "") typeName += $"({Size})";
        return typeName;
    }

    public void MaxSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has max size of {size}");
    public void MinSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has min size of {size}");

    public override bool IsValid(object? value) => true;
}
