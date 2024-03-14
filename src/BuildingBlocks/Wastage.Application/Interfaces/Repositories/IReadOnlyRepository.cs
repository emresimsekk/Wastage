using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Wastage.Application.Dynamic;
using Wastage.Application.Paging;
using Wastage.Core.Domain.Entities;

namespace Wastage.Application.Interfaces.Repositories;

public interface IReadOnlyRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
{
    Task<TEntity> GetAsync(
          Expression<Func<TEntity, bool>> predicate,
          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
          bool withDeleted = false,
          bool enableTracking = true,
          CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListAsync(
      Expression<Func<TEntity, bool>> predicate = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListByDynamicAsync(
       DynamicQuery dynamic,
       Expression<Func<TEntity, bool>> predicate = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
       int index = 0,
       int size = 10,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
       Expression<Func<TEntity, bool>> predicate = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default);
}
