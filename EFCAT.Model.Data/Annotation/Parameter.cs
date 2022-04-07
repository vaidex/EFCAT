using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class Parameter {
    public const EParameter Property = EParameter.PROPERTY;
    public const EParameter Method = EParameter.METHOD;

    public EParameter Type { get; private set; }
    public string Name { get; private set; }
    public object[]? Parameters { get; private set; } = null;

    public Parameter(EParameter type, string name, params object[] parameters) {
        Type = type;
        Name = name;
        Parameters = parameters;
    }

    public object? GetValue(ValidationContext context) => Type == EParameter.PROPERTY
        ? context.ObjectType.GetProperty(Name).GetValue(context.ObjectInstance)
        : context.ObjectType.GetMethods().FirstOrDefault(o => o.Name == Name && o.GetParameters().Select(p => new { Property = p, Index = Array.IndexOf(o.GetParameters().ToArray(), p) }).Where(p => p.Property.ParameterType == Parameters[p.Index].GetType()).Count() == Parameters.Count()).Invoke(context.ObjectInstance, Parameters);
}

public class PR {
    public const EParameter Property = EParameter.PROPERTY;
    public const EParameter Method = EParameter.METHOD;
}

public enum EParameter { PROPERTY, METHOD }