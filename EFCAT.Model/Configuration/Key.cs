namespace EFCAT.Model.Configuration;

internal class Key {
    public string Name { get; set; }
    public Type Type { get; set; }
    public Key(string name, Type type) {
        Name = name;
        Type = type;
    }
}