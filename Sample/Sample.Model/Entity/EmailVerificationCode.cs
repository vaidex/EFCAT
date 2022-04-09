
namespace Sample.Model.Entity;

[Table(DiscriminatorValue = "EMAIL")]
public class EmailVerificationCode : Code { }