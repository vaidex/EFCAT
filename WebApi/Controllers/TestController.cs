using Domain.Repository;
using EFCAT.Repository;
using EFCAT.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Entity;

namespace WebApi.Controllers {
    [ApiController]
    [Route("test")]
    public class TestController : AControllerAsync<TestEntity, int> {
        public TestController(ITestAsyncRepository repository) : base(repository) {
        }
    }
}
