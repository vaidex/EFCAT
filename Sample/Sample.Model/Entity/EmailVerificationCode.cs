
using EFCAT.Model.Annotation;

namespace Sample.Model.Entity;

[Table(DiscriminatorValue = "EMAIL")]
public class EmailVerificationCode : Code { }