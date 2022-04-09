namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PrimaryKeyAttribute : Attribute {
    public string[]? Keys { get; private set; } = null;

    public PrimaryKeyAttribute() { Keys = null; }
    public PrimaryKeyAttribute(params string[] keys) { Keys = keys; }
}
