using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Model.Annotation;

public class Column : ColumnAttribute {
    public Column(string name) : base(name) {
    }
}

