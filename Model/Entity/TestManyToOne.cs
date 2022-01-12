using EFCAT.Model.Annotation;

namespace Model.Entity {
    [Table("TEST_MTOS")]
    public class TestManyToOne {
        [PrimaryKey]
        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_ID")]
        public TestEntity TestEntity { get; set; }

        [ForeignColumn(ForeignType.MANY_TO_ONE, "TEST_SECOND_ID")]
        public TestEntity TestSecondEntity { get; set; }
    }
}
