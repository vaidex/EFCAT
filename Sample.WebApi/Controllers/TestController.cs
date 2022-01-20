using Sample.Domain.Repository;
using EFCAT.Domain.Repository;
using EFCAT.Service.Controller;
using Microsoft.AspNetCore.Mvc;
using Sample.Model.Entity;

namespace WebApi.Controllers;

[ApiController]
[Route("test")]
public class TestController : AControllerAsync<ITestAsyncRepository, User, int> {
    public TestController(ITestAsyncRepository repository) : base(repository) {
    }
}
