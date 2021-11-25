using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Annotation {
    public class Table : TableAttribute {
        public Table(string name) : base(name) {
        }
    }
}
