using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class Int32Attribute : TypeAttribute {
    public int Min { get => (int)(base.Min ?? 0); set => base.Min = value; }
    public int Max { get => (int)(base.Max ?? 0); set => base.Max = value; }

    public Int32Attribute() : base("int", Int32.MaxValue) { }
    protected Int32Attribute(string type, object size) : base(type, size) { }
}