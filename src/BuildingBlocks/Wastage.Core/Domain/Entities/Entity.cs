namespace Wastage.Core.Domain.Entities;
public class Entity<TId> : IEntityTimestamps
{
    public required TId Id { get; set; }
    public required DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public Entity() => Id = default;
    public Entity(TId id) => Id = id;
}
