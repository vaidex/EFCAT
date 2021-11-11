using EFCAT.Repository;
using Model.Configuration;
using Model.Entity;

namespace Domain.Repository {
    public interface ITestRepository : IRepository<TestEntity, int> { }

    public class TestRepository : ARepository<TestEntity, int>, ITestRepository {
        public TestRepository(TestDbContext context) : base(context) {
        }
    }
}
