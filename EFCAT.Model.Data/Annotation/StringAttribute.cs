using System.Text.RegularExpressions;

namespace EFCAT.Model.Data.Annotation;

public class StringAttribute : TypeAttribute {
    public long Min { get => (long)(base.Min ?? 0); set => base.Min = value; }
    public long Max { get => (long)(base.Max ?? 0); set => base.Max = value; }
    public Regex Pattern { get => base.Pattern; set => base.Pattern = value; }

    public StringAttribute() : base("string", Int32.MaxValue) { }
    protected StringAttribute(string type, object size) : base(type, size) { }
}