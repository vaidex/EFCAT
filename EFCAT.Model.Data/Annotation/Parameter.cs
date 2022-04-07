using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public enum EParameter { PROPERTY, METHOD }

public class Parameter {
    private EParameter? _type = null;
    private string? _name = null;
    private object[]? _parameter = null;

    public Parameter(EParameter type, string name, params object[] parameter) {
        _type = type;
        _name = name;
        _parameter = parameter;
    }

    public object? GetValue(ValidationContext context) => _type == EParameter.PROPERTY
        ? context.ObjectType.GetProperty(_name).GetValue(context.ObjectInstance)
        : context.ObjectType.GetMethods().FirstOrDefault(o => o.Name == _name && o.GetParameters().Select(p => new { Property = p, Index = Array.IndexOf(o.GetParameters().ToArray(), p) }).Where(p => p.Property.ParameterType == _parameter[p.Index].GetType()).Count() == _parameter.Count()).Invoke(context.ObjectInstance, _parameter);
}