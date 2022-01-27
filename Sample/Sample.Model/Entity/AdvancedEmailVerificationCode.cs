using EFCAT.Model.Annotation;
using EFCAT.Model.Data;

namespace Sample.Model.Entity;

[Table("ADVANCED_EMAIL_CODES")]
public class AdvancedEmailVerificationCode {
    [PrimaryKey]
    [ForeignColumn(EForeignType.MANY_TO_ONE, "CODE_ID")]
    public EmailVerificationCode Code { get; set; }

    [PrimaryKey]
    [Int(5, Min = 1, Max = 10)]
    public int Random { get; set; }

    public Image Image { get; set; }
}
