using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Model.Annotation;

public class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute {
    public ColumnAttribute(string name) : base(name) {
    }
}

