using Wastage.Application.Interfaces.Repositories;

namespace Wastage.Api.Domain.Entities;

public interface ICategoryWriteRepository : IWritableRepository<Category, Guid>
{

}
