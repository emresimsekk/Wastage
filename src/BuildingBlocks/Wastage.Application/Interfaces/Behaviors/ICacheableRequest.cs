namespace Wastage.Application.Interfaces.Behaviors;

public interface ICacheableRequest
{
    string CacheKey { get; }
    string CacheGroupKey { get; }
}
