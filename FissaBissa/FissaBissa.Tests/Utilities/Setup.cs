using System;
using FissaBissa.Data;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Tests.Utilities
{
    public static class Setup
    {
        public static T CreateService<T>(Func<ApplicationDbContext, T> create)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var database = new ApplicationDbContext(options);
            var service = create(database);

            database.SaveChanges();

            return service;
        }
    }
}
