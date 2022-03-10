
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Model.Data;

[NotMapped]
public abstract class ValueObject : IEquatable<object> {
    // Force the derived class to override these.
    public abstract override bool Equals(object? other);
    public abstract override int GetHashCode();

    // And then consistently use the overridden method as the implementation.
    public static bool operator ==(ValueObject objA, object objB) => objA.Equals(objB);
    public static bool operator !=(ValueObject objA, object objB) => !objA.Equals(objB);
}