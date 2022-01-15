using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class TypeAttribute : ValidationAttribute {
    public string Type { get; private set; }
    public string Size { get; private set; }

    public TypeAttribute(string type, string size = "") {
        Type = type;
        Size = size;
    }

    public TypeAttribute(string type, object size) : this(type, $"{size}") { }

    public string GetTypeName() {
        string typeName = Type;
        if (Size != "") typeName += $"({Size})";
        return typeName;
    }

    public void MaxSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has max size of {size}");
    public void MinSizeReached(string size) => throw new ArgumentOutOfRangeException($"{Type.ToUpper()} has min size of {size}");
}