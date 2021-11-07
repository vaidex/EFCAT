using EFCAT.Repository;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface ITestRepository : IRepository<TestEntity, int> { }

    public class TestRepository : ARepository<TestEntity, int>, ITestRepository {
        public TestRepository(TestDbContext context) : base(context) {
        }
    }
}
