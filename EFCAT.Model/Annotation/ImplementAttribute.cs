namespace EFCAT.Model.Annotation;

public class ImplementAttribute : Attribute {
    private string? _name;
    public ImplementAttribute(string? name = null) => _name = name;
    public string? GetName() => _name;
}
