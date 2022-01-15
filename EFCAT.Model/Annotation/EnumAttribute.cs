namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EnumAttribute : Attribute {
    public Type Type { get; set; }

    public EnumAttribute() { Type = typeof(string); }
    public EnumAttribute(Type type) { Type = type; }
}
