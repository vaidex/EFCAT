using EFCAT.Repository;
using Model.Configuration;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface ITestAsyncRepository : IRepositoryAsync<TestEntity, int> { }

    public class TestAsyncRepository : ARepositoryAsync<TestEntity, int>, ITestAsyncRepository {
        public TestAsyncRepository(TestDbContext context) : base(context) {
        }
    }
}
