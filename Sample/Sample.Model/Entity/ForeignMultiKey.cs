
namespace Sample.Model.Entity;

[Table("FOREIGN_MULTI_KEYS")]
public class ForeignMultiKey {
    [PrimaryKey]
    public int Id { get; set; }

    [PrimaryKey]
    public string Name { get; set; }

    [PrimaryKey]
    public DateOnly Date { get; set; }
}


[Table("FOREIGN_MULTI_KEYS_CUSTOMIZED")]
public class ForeignMultiKeyCustomized {
    [PrimaryKey("ID", "NAME")]
    [ForeignColumn(ForeignType.OneToOne, "ID", "NAME", "DATE")]
    public ForeignMultiKey Key { get; set; }
}

[Table("FOREIGN_MULTI_KEYS_DEFAULT")]
public class ForeignMultiKeyDefault {
    [PrimaryKey]
    [ForeignColumn(ForeignType.OneToOne, "ID", "NAME", "DATE")]
    public ForeignMultiKey Key { get; set; }
}