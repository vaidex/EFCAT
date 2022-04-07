using Microsoft.EntityFrameworkCore;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignColumnAttribute : Attribute {
    public EForeignType Type { get; set; }
    public string[] Keys { get; set; }
    public DeleteBehavior? OnDelete { get; set; }

    public ForeignColumnAttribute(EForeignType type = ForeignType.OneToOne, string keys = "") : this(type, (keys != "" ? keys.Replace(" ", "").Split(",") : new string[0])) { }
    public ForeignColumnAttribute(EForeignType type = ForeignType.OneToOne, params string[] keys) {
        Type = type;
        Keys = keys;
    }
}

public class ForeignType {
    public const EForeignType OneToOne = EForeignType.ONE_TO_ONE;
    public const EForeignType ManyToOne = EForeignType.MANY_TO_ONE;
}

public class FK {
    public const EForeignType OneToOne = ForeignType.OneToOne;
    public const EForeignType ManyToOne = ForeignType.ManyToOne;
}

public enum EForeignType { ONE_TO_ONE, MANY_TO_ONE }
