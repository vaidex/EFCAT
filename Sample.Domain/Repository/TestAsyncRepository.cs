using EFCAT.Domain.Repository;
using Sample.Model.Configuration;
using Sample.Model.Entity;

namespace Sample.Domain.Repository;

public interface ITestAsyncRepository : IRepositoryAsync<User> { }

public class TestAsyncRepository : ARepositoryAsync<User>, ITestAsyncRepository {
    public TestAsyncRepository(TestDbContext context) : base(context) {
    }
}