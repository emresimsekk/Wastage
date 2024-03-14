using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wastage.Application.Behaviors.Caching;
using Wastage.Application.Behaviors.Logging;
using Wastage.Application.Behaviors.Transaction;
using Wastage.Application.Behaviors.Validation;
using Wastage.Application.Serilog;
using Wastage.Application.Settings;

namespace Wastage.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        AddCachingBehavior(services, configuration);
        AddTransactionScopeBehavior(services);
        AddRequestValidationBehavior(services);
        AddLoggingBehavior(services);
        AddSerilog();
        return services;
    }
    private static void AddCachingBehavior(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<CacheSettings>(opt =>
         {
             configuration.GetSection("CacheSettings").Bind(opt);
         });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("CacheSettings:Connection").Value;
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
    }
    private static void AddTransactionScopeBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));
    }
    private static void AddRequestValidationBehavior(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
    private static void AddLoggingBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
    private static void AddSerilog()
    {
        SerilogExtensions.AddCustomSerilog();
    }
}
