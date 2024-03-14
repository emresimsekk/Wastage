using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastage.Api.Domain.Entities;

namespace Wastage.Api.Persistence.Data.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(b => b.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(b => b.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(b => b.UpdatedDate)
                .HasColumnName("UpdatedDate");

            builder.Property(b => b.DeletedDate)
                .HasColumnName("DeletedDate");

            builder.HasIndex(b => b.Name, name: "UK_Categories_Name")
                .IsUnique();

            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

            builder.ToTable("Categories")
                .HasKey(b => b.Id);
        }
    }
}
