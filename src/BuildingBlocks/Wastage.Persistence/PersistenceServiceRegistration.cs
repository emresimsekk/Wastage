using Microsoft.Extensions.DependencyInjection;
namespace Wastage.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }
}
