using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EFCAT.Service.Authentication;

public static class AuthenticationServiceExtension {
    public static IServiceCollection AddAuthenticationService<TService>(this IServiceCollection services) where TService : AuthenticationStateProvider
        => services.AddScoped<AuthenticationStateProvider, TService>();
}
