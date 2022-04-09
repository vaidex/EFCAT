
namespace Sample.Model.Entity;

public class Role {
    [PrimaryKey]
    [ForeignColumn(EForeignType.MANY_TO_ONE, "USER_ID")]
    public User User { get; set; }

    [PrimaryKey]
    public string RoleName { get; set; }
}
