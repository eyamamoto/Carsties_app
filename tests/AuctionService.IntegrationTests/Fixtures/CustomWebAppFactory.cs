using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace AuctionService.IntegrationTests.Fixtures
{
    //cria uma instancia da nossa aplicação que permite reutilizar durante as outras execuções dos testes
    //high value tests
    public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

        //quando a classe inicializa
        public async Task InitializeAsync()
        {
            //inicia o test container do postgres
            await _postgreSqlContainer.StartAsync();
        }

        //configurar os serviços do test container
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //postgres
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<AuctionDbContext>(options =>
                {
                    options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
                });

                //rabbitmq
                services.AddMassTransitTestHarness();

                //migration database
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopeServices = scope.ServiceProvider;
                var db = scopeServices.GetRequiredService<AuctionDbContext>();
                db.Database.Migrate();
            });
        }

        //quando a classe finaliza
        Task IAsyncLifetime.DisposeAsync()
        {
            //finaliza o test container 
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
