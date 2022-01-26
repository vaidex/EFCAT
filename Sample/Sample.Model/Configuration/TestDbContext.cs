using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Entity;

namespace Sample.Model.Configuration;

public class TestDbContext : DatabaseContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Code> UserCodes { get; set; }
    public DbSet<EmailVerificationCode> EmailCodes { get; set; }
    public DbSet<AdvancedEmailVerificationCode> AdvancedEmailCodes { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options, true) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }

}
