using Microsoft.EntityFrameworkCore;
using Wastage.Api.Domain.Entities;
using Wastage.Api.Persistence.Data.DbContexts;
using Wastage.Persistence;
namespace Wastage.Api.Persistence.Data.DI
{
    public static class PersistenceModule
    {
        public static void AddPersistenceModule(this IServiceCollection services, IConfiguration configuration)
        {
            SetReadOnlyDbContext(services, configuration);
            SetWritableDbContext(services, configuration);
            AddRepositories(services);
            SetPersistenceServices(services);

          
        }
        private static void SetReadOnlyDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var readOnlyDbConnectionString = configuration.GetConnectionString("ReadonlyDb");
            services.AddDbContext<ReadOnlyDbContext>(options =>
            {
                options.UseNpgsql(readOnlyDbConnectionString);
                options.EnableSensitiveDataLogging();
            });
        }
        private static void SetWritableDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var writableDbConnectionString = configuration.GetConnectionString("WritableDb");
            services.AddDbContext<WritableDbContext>(options =>
            {
                options.UseNpgsql(writableDbConnectionString);
                options.EnableSensitiveDataLogging();
            });
        }
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
        }
        private static void SetPersistenceServices(IServiceCollection services)
        {
            services.AddPersistenceServices();
        }
    }
}
