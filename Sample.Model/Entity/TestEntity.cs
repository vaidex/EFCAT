using EFCAT.Model.Annotation;
using EFCAT.Model.Data;

namespace Sample.Model.Entity;

[Table("TEST_ENTITIES")]
public class TestEntity {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [Unique]
    [Varchar(16, Min = 3, Max = 16, Nullable = false, ErrorMessage = "You need to have between @min and @max characters.")]
    public string Name { get; set; }

    [Required]
    public Crypt<SHA256> Password { get; set; }

    // Number from 0-100 saved as DECIMAL(5, 2) which is equal to 999.99
    [Decimal(3, 2, Min = 10, Max = 20)]
    public decimal Number { get; set; }

    [Enum(typeof(string))]
    public ETypes Type { get; set; }

    //public Image Image { get; set; }

    //public Json<TestJson> Json { get; set; }

    public ICollection<TestManyToOne>? TestManyToOnes { get; set; }

    [ReferenceColumn("TestSecondEntity")]
    public ICollection<TestManyToOne>? TestSecondManyToOnes { get; set; }
}

[NotGenerated]
public class TestJson {
    public string Name { get; set; }
    public string Description { get; set; }
}

public enum ETypes { ON, OFF, IDLE }