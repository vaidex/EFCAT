using Microsoft.EntityFrameworkCore;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ReferenceColumnAttribute : Attribute {
    public readonly string property;

    public ReferenceColumnAttribute(string property = "") {
        this.property = property;
    }


}
