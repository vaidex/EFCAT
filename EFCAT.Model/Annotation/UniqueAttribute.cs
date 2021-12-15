namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : Attribute {

    public string? ErrorMessage { get; set; } = null;

    public UniqueAttribute() {

    }
}