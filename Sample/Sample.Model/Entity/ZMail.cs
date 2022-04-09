
namespace Sample.Model.Entity;

[Table("ZMAILS")]
public class ZMail {
    [PrimaryKey]
    [ForeignColumn(EForeignType.ONE_TO_ONE, "USER_ID")]
    public User User { get; set; }

    [Varchar(32, Min = 5)]
    public string Value { get; set; }
}