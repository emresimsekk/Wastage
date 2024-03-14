using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Wastage.Application.Serilog;

public static class SerilogExtensions
{
    public static void AddCustomSerilog()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        var applicationName = Assembly.GetExecutingAssembly().GetName().Name;   

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", $"{applicationName} - {environment}")
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .WriteTo.Async(writeTo=>writeTo.Console())
            .WriteTo.Async(writeTo => writeTo.Debug())
            .WriteTo.Async(writeTo => writeTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{applicationName.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                OverwriteTemplate = true,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                                           EmitEventFailureHandling.WriteToFailureSink |
                                                           EmitEventFailureHandling.RaiseCallback |
                                                           EmitEventFailureHandling.ThrowException
            }))
           .CreateLogger();

    }
}
