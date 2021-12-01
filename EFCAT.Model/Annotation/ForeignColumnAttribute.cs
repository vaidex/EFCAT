using Microsoft.EntityFrameworkCore;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignColumnAttribute : Attribute {
    public readonly ForeignType type;
    public readonly string[] keys;
    public readonly DeleteBehavior onDelete;

    public ForeignColumnAttribute(ForeignType type = ForeignType.ONE_TO_ONE, string keys = "", DeleteBehavior onDelete = DeleteBehavior.Cascade) {
        this.type = type;
        this.keys = (keys != "" ? keys.Replace(" ", "").Split(",") : new string[0]);
        this.onDelete = onDelete;
    }


}

public enum ForeignType { ONE_TO_ONE, MANY_TO_ONE }
