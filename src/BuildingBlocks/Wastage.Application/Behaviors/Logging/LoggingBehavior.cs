using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Wastage.Application.Interfaces.Behaviors;

namespace Wastage.Application.Behaviors.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggerableRequest
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Request
        _logger.LogInformation($"Executing {typeof(TRequest).Name} operation...");
        Type myType = request.GetType();

        //Get request values
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            object propValue = prop.GetValue(request, null);
            _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
        }

        var response = await next();
        //Response
        _logger.LogInformation($"Response Type : {typeof(TResponse).Name}");
        return response;
    }
}
