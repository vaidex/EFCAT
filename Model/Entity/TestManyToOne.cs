using EFCAT.Annotation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity {
    [Table("TEST_MTOS")]
    public class TestManyToOne {
        [PrimaryKey]
        [ForeignColumn(ForeignType.ONE_TO_ONE, "TEST_ID")]
        public TestEntity TestEntity { get; set; }
    }
}
