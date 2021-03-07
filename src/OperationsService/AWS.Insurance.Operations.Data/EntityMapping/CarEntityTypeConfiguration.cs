using AWS.Insurance.Operations.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWS.Insurance.Operations.Data.EntityMapping
{
    public class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.BranchType)
                .IsRequired();
            builder.Property(e => e.Year)
                .IsRequired();
            builder.Property(e => e.CustomerId)
                .IsRequired();
            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.CustomerId);

            builder.Property(e => e.RowVersion).IsRowVersion();
        }
    }
}