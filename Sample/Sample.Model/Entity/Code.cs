using EFCAT.Model.Annotation;

namespace Sample.Model.Entity;

[Table("USER_HAS_CODES")]
public class Code {
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignColumn(EForeignType.MANY_TO_ONE, "USER_ID")]
    public User User { get; set; }

    [Enum(typeof(string))]
    public CodeType Type { get; set; }

    public string Value { get; set; }

    public DateTime ExpiresAt { get; set; }
}

public enum CodeType {
    EMAIL_VERIFICATION, PASSWORD_RESET
}