using EFCAT.Domain.Repository;
using Sample.Model.Configuration;
using Sample.Model.Entity;

namespace Sample.Domain.Repository;

public interface ITestRepository : IRepository<TestEntity, int> { }

public class TestRepository : ARepository<TestEntity, int>, ITestRepository {
    public TestRepository(TestDbContext context) : base(context) {
    }
}