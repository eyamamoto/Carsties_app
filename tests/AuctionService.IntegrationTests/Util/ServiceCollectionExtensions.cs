using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.IntegrationTests.Util
{
    public static class ServiceCollectionExtensions
    {
        public static void RemoveDbContext<T>(this IServiceCollection services)
        {
            //postgres
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

            if (descriptor != null) services.Remove(descriptor);
        }

        public static void EnsureCreated<T>(this IServiceCollection services)
        {
            //migration database
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopeServices = scope.ServiceProvider;
            var db = scopeServices.GetRequiredService<AuctionDbContext>();
            db.Database.Migrate();

            //seed banco de dados
            DbHelper.InitDbForTests(db);
        }
    }
}
