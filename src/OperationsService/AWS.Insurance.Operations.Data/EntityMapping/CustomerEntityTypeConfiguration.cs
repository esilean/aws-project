using AWS.Insurance.Operations.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AWS.Insurance.Operations.Data.EntityMapping
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(e => e.CNumber)
                .IsRequired();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(60);
            builder.Property(e => e.Age)
                .IsRequired();
            builder.Property(e => e.Dob)
                .IsRequired();

            builder.Property<DateTime>("DateCreated")
               .IsRequired()
               .ValueGeneratedOnAdd()
               .HasDefaultValue(DateTime.Now);

            builder.Property(e => e.RowVersion).IsRowVersion();

            builder.HasMany(e => e.Cars);
        }
    }
}