using Wastage.Api.Persistence.Data.DbContexts;
using Wastage.Persistence.Repositories;

namespace Wastage.Api.Domain.Entities;

public class CategoryWriteRepository : WritableRepository<Category, Guid, ReadOnlyDbContext>, ICategoryWriteRepository
{
    public CategoryWriteRepository(ReadOnlyDbContext context) : base(context)
    {
    }
}
