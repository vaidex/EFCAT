namespace EFCAT.Model.Annotation;

public class ImplementAttribute : Attribute {
    public string? Name { get; private set; }
    public ImplementAttribute(string? name = null) => Name = name;
}
