using EFCAT.Domain.Repository;
using Model.Configuration;
using Model.Entity;

namespace Domain.Repository;

public interface ITestAsyncRepository : IRepositoryAsync<TestEntity, int> { }

public class TestAsyncRepository : ARepositoryAsync<TestEntity, int>, ITestAsyncRepository {
    public TestAsyncRepository(TestDbContext context) : base(context) {
    }
}