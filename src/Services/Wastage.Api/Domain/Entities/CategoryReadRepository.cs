using Wastage.Api.Persistence.Data.DbContexts;
using Wastage.Persistence.Repositories;

namespace Wastage.Api.Domain.Entities;

public class CategoryReadRepository : WritableRepository<Category, Guid, ReadOnlyDbContext>, ICategoryReadRepository
{
    public CategoryReadRepository(ReadOnlyDbContext context) : base(context)
    {
    }
}
