using EFCAT.Model.Annotation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity {
    [System.ComponentModel.DataAnnotations.Schema.Table("TEST_MTOS")]
    public class TestManyToOne {
        [PrimaryKey]
        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_ID")]
        public TestEntity TestEntity { get; set; }

        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_SECOND_ID")]
        public TestEntity TestSecondEntity { get; set; }
    }
}
