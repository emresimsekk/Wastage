using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Wastage.Application.Dynamic;
using Wastage.Application.Dynamic.Extensions;
using Wastage.Application.Interfaces.Repositories;
using Wastage.Application.Paging;
using Wastage.Application.Paging.Extensions;
using Wastage.Core.Domain.Entities;

namespace Wastage.Persistence.Repositories;

public class ReadOnlyRepository<TEntity, TEntityId, TContext> : IReadOnlyRepository<TEntity, TEntityId>
        where TEntity : Entity<TEntityId>
        where TContext : DbContext
{
    protected readonly TContext _context;

    public ReadOnlyRepository(TContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        if (predicate != null)
            queryable = queryable.Where(predicate);

        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public IQueryable<TEntity> Query() => _context.Set<TEntity>();


}
