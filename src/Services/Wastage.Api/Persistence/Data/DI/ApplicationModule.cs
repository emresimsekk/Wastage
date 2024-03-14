using Wastage.Application;
namespace Wastage.Api.Persistence.Data.DI
{
    public static class ApplicationModule
    { 
        public static void AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
        {
            SetApplicationServices(services,configuration);
        }
        private static void SetApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAplicationServices(configuration);
        }

    }
}
