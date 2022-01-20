using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Entity;

namespace Sample.Model.Configuration;

public class TestDbContext : DatabaseContext {
    public DbSet<User> Users { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }

}
