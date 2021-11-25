using EFCAT.Model.Annotation;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity {
    [Table("TEST_ENTITIES")]
    public class TestEntity {
        [PrimaryKey]
        [AutoIncrement]
        [Column("ID")]
        public int Id { get; set; }

        [Unique]
        [Required]
        [MaxLength(20)]
        [Column("NAME")]
        public string Name { get; set; }

        [Encrypt]
        [Required]
        [MaxLength(256)]
        [Column("PASSWORD")]
        public string Password { get; set; }

        // Number from 0-100 saved as NUMBER(5, 2) which is equal to 999.99
        [Number(min: 0, max: 100, digits: 3, decimals: 2)]
        [Column("NUMBER")]
        public double Number { get; set; }

        public ICollection<TestManyToOne>? TestManyToOnes { get; set; }
    }
}
