using Wastage.Core.Domain.Entities;

namespace Wastage.Api.Domain.Entities
{
    public class Category : Entity<Guid>
    {
        public required string Name { get; set; }
    }
}
