using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BasicApp.Infrastructure.DI;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var allTypes = assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        // Register all *Repository classes as their implemented interfaces
        var repoTypes = allTypes
            .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal));

        foreach (var implType in repoTypes)
        {
            var interfaces = implType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                services.AddScoped(@interface, implType);
            }
        }

        // Register all *Service classes as their implemented interfaces (if you add any)
        var serviceTypes = allTypes
            .Where(t => t.Name.EndsWith("Service", StringComparison.Ordinal));

        foreach (var implType in serviceTypes)
        {
            var interfaces = implType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                services.AddScoped(@interface, implType);
            }
        }

        return services;
    }
}
