using Wastage.Core.Domain.Entities;

namespace Wastage.Application.Interfaces.Repositories;

public interface IWritableRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
{
    Task<TEntity> AddAsync(TEntity entity);

    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);

    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
}
