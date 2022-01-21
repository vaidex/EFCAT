using EFCAT.Domain.Repository;
using Sample.Model.Configuration;
using Sample.Model.Entity;

namespace Sample.Domain.Repository;

public interface ITestRepository : IRepository<User> { }

public class TestRepository : ARepository<User>, ITestRepository {
    public TestRepository(TestDbContext context) : base(context) {
    }
}