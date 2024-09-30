using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    public class AuctionControllerIntegrationTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory factory;
        private readonly HttpClient httpclient;
        //a cada teste o banco de dados deve ser limpo e reiniciado

        public AuctionControllerIntegrationTests(CustomWebAppFactory factory)
        {
            this.factory = factory;
            httpclient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAuctions_ShouldReturn3Auctions()
        {
            //arrange

            //act
            var response = await httpclient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");

            //assert
            Assert.Equal(10, response.Count());
        }


        //antes de cada teste esse metodo é executado
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }


        //depois de cada teste esse metodo é executado
        public Task DisposeAsync()
        {
           
            //reinicia o banco de dados depois de cadas teste
            //using var scope = factory.Services.CreateScope();
            //var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
            //DbHelper.ReinitDbForTests(db);
            return Task.CompletedTask;
        }

        
        
    }
}