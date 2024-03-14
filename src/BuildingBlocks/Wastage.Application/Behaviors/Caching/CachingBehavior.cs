using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Wastage.Application.Interfaces.Behaviors;
using Wastage.Application.Settings;
namespace Wastage.Application.Behaviors.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheableRequest
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _cacheSettings;

    public CachingBehavior(IDistributedCache cache, IOptions<CacheSettings> cacheSettings)
    {
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_cacheSettings.Enabled)
            return await next();

        TResponse response;
        Byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
        }
        else
        {
            response = await getResponseAndAddToCache(request, next, cancellationToken);
        }

        return response;
    }
    private async Task<TResponse?> getResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next();

        TimeSpan slidingExpiration = TimeSpan.FromDays(_cacheSettings.SlidingExpiration);

        DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };
        Byte[] serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

        await _cache.SetAsync(request.CacheKey, serializedData, cacheOptions, cancellationToken);

        if (request.CacheGroupKey != null)
            await addCacheKeyToGroup(request, slidingExpiration, cancellationToken);

        //_logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");
        return response;
    }

    private async Task addCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        byte[]? cacheGroupCache = await _cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);
        HashSet<string> cacheKeysInGroup;
        if (cacheGroupCache != null)
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
            if (!cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
        }
        else
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
        byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

        byte[]? cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            cancellationToken
        );
        int? cacheGroupCacheSlidingExpirationValue = null;
        if (cacheGroupCacheSlidingExpirationCache != null)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));
        if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);
        byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

        DistributedCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        await _cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);

        await _cache.SetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        );
    }

}
