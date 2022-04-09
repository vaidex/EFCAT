
namespace Sample.Model.Entity;

[NotGenerated]
public class NotGeneratedKey {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    [Varchar(32)]
    public string Name { get; set; }
}

[Table("NOT_GENERATED_KEY_INHERIT")]
public class NotGeneratedKeyInherit : Person {

}

[Table("NOT_GENERATED_KEY_IMPLEMENT")]
public class NotGeneratedKeyImplement {
    [PrimaryKey]
    [Implement]
    public Person Person { get; set; }
}