
namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableAttribute : Attribute {
    public string Name { get; set; }
    public string? Schema { get; set; }
    public bool Discriminator { get; set; }
    public string? DiscriminatorValue { get; set; }

    public TableAttribute() { }
    public TableAttribute(string name) {
        Name = name;
        Discriminator = false;
    }
}

