using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        
        // Add Firebase Authentication Service
        services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
        
        return services;
    }
}
