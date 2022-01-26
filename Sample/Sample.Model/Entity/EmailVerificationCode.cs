
using EFCAT.Model.Annotation;

namespace Sample.Model.Entity;

[Table("EMAIL_CODES")]
public class EmailVerificationCode : Code {
    public EmailVerificationCode() { Type = CodeType.EMAIL_VERIFICATION; }
}