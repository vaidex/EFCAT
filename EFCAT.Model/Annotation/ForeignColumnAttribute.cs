using Microsoft.EntityFrameworkCore;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignColumnAttribute : Attribute {
    public EForeignType Type { get; set; }
    public string[] Keys { get; set; }
    public DeleteBehavior? OnDelete { get; set; }

    public ForeignColumnAttribute(EForeignType type = EForeignType.ONE_TO_ONE, string keys = "") : this(type, (keys != "" ? keys.Replace(" ", "").Split(",") : new string[0])) { }
    public ForeignColumnAttribute(EForeignType type = EForeignType.ONE_TO_ONE, params string[] keys) {
        Type = type;
        Keys = keys;
    }
}

public enum EForeignType { ONE_TO_ONE, MANY_TO_ONE }
