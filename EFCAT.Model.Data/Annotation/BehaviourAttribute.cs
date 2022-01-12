using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

internal class BehaviourAttribute : Attribute {
    /// <summary>
    ///   The dynamically created instance of the type passed to the constructor.
    /// </summary>
    public object DynamicInstance { get; private set; }

    /// <summary>
    ///   Create a new attribute and initialize a certain type.
    /// </summary>
    /// <param name = "dynamicType">The type to initialize.</param>
    /// <param name = "constructorArguments">The arguments to pass to the constructor of the type.</param>
    public BehaviourAttribute(Type dynamicType, params object[] constructorArguments) {
        DynamicInstance = Activator.CreateInstance(dynamicType, constructorArguments);
    }
}

internal class BehaviourValidationAttribute : ValidationAttribute {
    /// <summary>
    ///   The dynamically created instance of the type passed to the constructor.
    /// </summary>
    public object DynamicInstance { get; private set; }

    /// <summary>
    ///   Create a new attribute and initialize a certain type.
    /// </summary>
    /// <param name = "dynamicType">The type to initialize.</param>
    /// <param name = "constructorArguments">The arguments to pass to the constructor of the type.</param>
    public BehaviourValidationAttribute(Type dynamicType, params object[] constructorArguments) {
        DynamicInstance = Activator.CreateInstance(dynamicType, constructorArguments);
    }
}