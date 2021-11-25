using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Model.Annotation;

public class Table : TableAttribute {
    public Table(string name) : base(name) {
    }
}

