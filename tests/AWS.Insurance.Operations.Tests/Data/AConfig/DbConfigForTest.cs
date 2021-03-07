using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;

namespace AWS.Insurance.Operations.Tests.Data.AConfig
{

    public class DbConfigForTest : IDisposable
    {
        protected DbContextOptions<OpDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;

        public DbConfigForTest()
        {
            ContextOptions = new DbContextOptionsBuilder<OpDbContext>()
                .UseSqlite(CreateInMemoryDB())
                .Options;

           _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;

            Seed();
        }

        private static DbConnection CreateInMemoryDB()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public void Dispose() => _connection.Dispose();

        private void Seed()
        {
            using var context = new OpDbContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var customer = new Customer(9999, "name", 33, DateTime.Now, 9999);
            customer.AddZone(Zone.Red);
            context.Add(customer);

            var car = new Car(customer.Id, BranchType.BMW, "ZZ", 1900);
            context.Add(car);

            context.SaveChanges();

        }


    }
}
