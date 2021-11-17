using EFCAT.Configuration;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Model.Configuration {
    public class TestDbContext : DatabaseContext {
        public DbSet<TestEntity> TestEntities { get; set; }
        public DbSet<TestManyToOne> TestMTOS { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

    }
}
