using Domain.Repository;
using EFCAT.Domain.Repository;
using EFCAT.Service.Controller;
using Microsoft.AspNetCore.Mvc;
using Model.Entity;

namespace WebApi.Controllers;

[ApiController]
[Route("test")]
public class TestController : AControllerAsync<ITestAsyncRepository, TestEntity, int> {
    public TestController(ITestAsyncRepository repository) : base(repository) {
    }
}
