using AWS.Insurance.Operations.Data.EntityMapping;
using AWS.Insurance.Operations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AWS.Insurance.Operations.Data.Context
{
    public class OpDbContext : DbContext
    {
        public OpDbContext(DbContextOptions<OpDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
            builder.ApplyConfiguration(new CarEntityTypeConfiguration());
        }
    }
}