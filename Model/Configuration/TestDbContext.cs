using EFCAT.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Configuration {
    public class TestDbContext : DatabaseContext {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) {
        }
    }
}
