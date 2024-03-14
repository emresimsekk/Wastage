using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Wastage.Application.Interfaces.Behaviors;
using Wastage.Application.Settings;


namespace Wastage.Application.Behaviors.Caching;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _cacheSettings;
    public CacheRemovingBehavior(IDistributedCache cache, IOptions<CacheSettings> cacheSettings)
    {
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_cacheSettings.Enabled)
            return await next();

        TResponse response = await next();

        if (request.CacheGroupKey != null)
        {
            byte[]? cachedGroup = await _cache.GetAsync(request.CacheGroupKey, cancellationToken);
            if (cachedGroup != null)
            {
                HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup))!;
                foreach (string key in keysInGroup)
                {
                    await _cache.RemoveAsync(key, cancellationToken);
                }

                await _cache.RemoveAsync(request.CacheGroupKey, cancellationToken);
                await _cache.RemoveAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken);
            }
        }

        if (request.CacheKey != null)
            await _cache.RemoveAsync(request.CacheKey, cancellationToken);


        return response;
    }
}
