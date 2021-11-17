using EFCAT.Annotation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity {
    [Table("TEST_MTOS")]
    public class TestManyToOne {
        [PrimaryKey]
        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_ID, TEST_NAME")]
        public TestEntity TestEntity { get; set; }
    }
}
