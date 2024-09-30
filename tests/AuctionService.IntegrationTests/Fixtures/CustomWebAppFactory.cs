using AuctionService.Data;
using AuctionService.IntegrationTests.Util;
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
using WebMotions.Fake.Authentication.JwtBearer;

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
                services.RemoveDbContext<AuctionDbContext>();

                services.AddDbContext<AuctionDbContext>(options =>
                {
                    options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
                });

                //rabbitmq
                services.AddMassTransitTestHarness();

                //migration database && seed database
                services.EnsureCreated<AuctionDbContext>();


                //cria autenticação
                services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
                    .AddFakeJwtBearer(opt =>
                    {
                        opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt;
                    });
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
