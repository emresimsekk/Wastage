using Wastage.Application.Interfaces.Repositories;

namespace Wastage.Api.Domain.Entities;

public interface ICategoryReadRepository : IReadOnlyRepository<Category, Guid>
{

}
