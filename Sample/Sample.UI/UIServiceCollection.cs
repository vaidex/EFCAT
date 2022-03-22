using Sample.Domain.Repository;
using EFCAT.Service.Authentication;
using EFCAT.Service.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Model.Configuration;
using Sample.UI.Services;

namespace Sample.UI;

public static class UIServiceCollection {
    public static IServiceCollection AddUI(this IServiceCollection services) {
        services.AddScoped<ITestAsyncRepository, TestAsyncRepository>();

        services.AddDbContext<TestDbContext>(
            options => options
            .UseMySql("server = localhost; port = 3306; database = efcat_db; user = htl; password = insy; Persist Security Info = False; Connect Timeout = 300", new MySqlServerVersion(new System.Version("8.0.25")))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
        );

        services.AddHttpClient();
        services.AddLocalStorage();
        services.AddAuthenticationService<MyAuthenticationService>();

        return services;
    }
}