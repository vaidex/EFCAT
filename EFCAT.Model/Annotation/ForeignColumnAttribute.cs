using Microsoft.EntityFrameworkCore;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignColumnAttribute : Attribute {
    public ForeignType Type { get; set; }
    public string[] Keys { get; set; }
    public DeleteBehavior OnDelete { get; set; } = DeleteBehavior.Cascade;

    public ForeignColumnAttribute(ForeignType type = ForeignType.ONE_TO_ONE, params string[] keys) {
        Type = type;
        Keys = keys;
    }

    public ForeignColumnAttribute(ForeignType type = ForeignType.ONE_TO_ONE, string keys = "") : this(type, (keys != "" ? keys.Replace(" ", "").Split(",") : new string[0])) { }


}

public enum ForeignType { ONE_TO_ONE, MANY_TO_ONE }
