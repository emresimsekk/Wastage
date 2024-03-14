namespace Wastage.Application.Interfaces.Behaviors;
public interface ICacheRemoverRequest
{
    string CacheKey { get; }
    string CacheGroupKey { get; }
}
