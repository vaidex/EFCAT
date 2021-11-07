using EFCAT.Annotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity {
    [Table("TEST_MTOS")]
    public class TestManyToOne {
        [PrimaryKey]
        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_ID")]
        public TestEntity Entity { get; set; }
    }
}
