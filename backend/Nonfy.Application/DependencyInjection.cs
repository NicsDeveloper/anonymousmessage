using Microsoft.Extensions.DependencyInjection;
using Nonfy.Application.UseCases.RegisterUser;

namespace Nonfy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserHandler>();
        return services;
    }
}
