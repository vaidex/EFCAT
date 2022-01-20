using EFCAT.Domain.Repository;
using Sample.Model.Configuration;
using Sample.Model.Entity;

namespace Sample.Domain.Repository;

public interface ITestRepository : IRepository<User, int> { }

public class TestRepository : ARepository<User, int>, ITestRepository {
    public TestRepository(TestDbContext context) : base(context) {
    }
}