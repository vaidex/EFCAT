
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
public class NotGeneratedKeyInherit : NotGeneratedKey {
    [PrimaryKey]
    public int SecondId { get; set; }
}

[Table("NOT_GENERATED_KEY_INHERIT_2ND")]
public class NotGeneratedKeyInherit2nd : NotGeneratedKey {
    [PrimaryKey]
    public int SecondId { get; set; }
}

[Table("NOT_GENERATED_KEY_IMPLEMENT")]
public class NotGeneratedKeyImplement {
    [PrimaryKey]
    public int SecondId { get; set; }

    [Implement]
    public NotGeneratedKey NotGeneratedKey { get; set; }
}