using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Entity;

namespace Sample.Model.Configuration;

public class TestDbContext : DatabaseContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Code> UserCodes { get; set; }
    public DbSet<EmailVerificationCode> EmailCodes { get; set; }
    public DbSet<AdvancedEmailVerificationCode> AdvancedEmailCodes { get; set; }
    public DbSet<ZMail> ZMails { get; set; }
    public DbSet<NicePerson> NicePeople { get; set; }
    public DbSet<BadPerson> BadPeople { get; set; }
    public DbSet<Role> Roles { get; set; }

    // Select the primary key from a foreign column
    public DbSet<ForeignMultiKey> ForeignMultiKeys { get; set; }
    public DbSet<ForeignMultiKeyCustomized> ForeignMultiKeyCustomized { get; set; }
    public DbSet<ForeignMultiKeyDefault> ForeignMultiKeyDefault { get; set; }

    // Use a not generated base class as primary key
    public DbSet<NotGeneratedKeyInherit> NotGeneratedKeyInherits { get; set; }
    public DbSet<NotGeneratedKeyInherit2nd> NotGeneratedKeyInherit2nds { get; set; }
    public DbSet<NotGeneratedKeyImplement> NotGeneratedKeyImplements { get; set; }


    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options, true) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }

}
