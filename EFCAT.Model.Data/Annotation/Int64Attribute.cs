using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Data.Annotation;

public class Int64Attribute : TypeAttribute {
    public long Min { get => (long)(base.Min ?? 0); set => base.Min = value; }
    public long Max { get => (long)(base.Max ?? 0); set => base.Max = value; }

    public Int64Attribute() : base("long", Int64.MaxValue) { }
    protected Int64Attribute(string type, object size) : base(type, size) { }}