using EFCAT.Domain.Repository;
using Sample.Model.Configuration;
using Sample.Model.Entity;

namespace Sample.Domain.Repository;

public interface ITestAsyncRepository : IRepositoryAsync<User, int> { }

public class TestAsyncRepository : ARepositoryAsync<User, int>, ITestAsyncRepository {
    public TestAsyncRepository(TestDbContext context) : base(context) {
    }
}