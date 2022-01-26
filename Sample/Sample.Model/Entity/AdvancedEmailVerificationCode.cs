using EFCAT.Model.Annotation;
using EFCAT.Model.Data;

namespace Sample.Model.Entity;

[Table("ADVANCED_EMAIL_CODES")]
public class AdvancedEmailVerificationCode {
    [PrimaryKey]
    [ForeignColumn(EForeignType.MANY_TO_ONE, "CODE_ID")]
    public EmailVerificationCode Code { get; set; }

    public Image Image { get; set; }
}
