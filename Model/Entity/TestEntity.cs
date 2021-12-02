using EFCAT.Model.Annotation;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity {
    [Table("TEST_ENTITIES")]
    public class TestEntity {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Unique]
        [Required]
        [Varchar(16, Min = 3, ErrorMessage = "You need to have between @min and @max characters.")]
        public string Name { get; set; }

        [Encrypt]
        [Required]
        [Varchar(256)]
        public string Password { get; set; }

        // Number from 0-100 saved as DECIMAL(5, 2) which is equal to 999.99
        [Decimal(3, 2, Min = 10, Max = 20)]
        public decimal Number { get; set; }

        public ICollection<TestManyToOne>? TestManyToOnes { get; set; }

        [ReferenceColumn("TestSecondEntity")]
        public ICollection<TestManyToOne>? TestSecondManyToOnes { get; set; }
    }
}
