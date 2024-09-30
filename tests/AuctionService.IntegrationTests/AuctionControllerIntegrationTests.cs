using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    public class AuctionControllerIntegrationTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory factory;
        private readonly HttpClient httpclient;
        private const string Model_T_ID = "3659ac24-29dd-407a-81f5-ecfe6f924b9b";

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

        [Fact]
        public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
        {
            //arrange

            //act
            var response = await httpclient.GetFromJsonAsync<AuctionDto>($"api/auctions/{Model_T_ID}");

            //assert
            Assert.Equal("Model T", response.Model);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidId_ShouldReturn404()
        {
            //arrange

            //act
            var response = await httpclient.GetAsync($"api/auctions/{Guid.NewGuid()}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidIdGuid_ShouldReturn400()
        {
            //arrange

            //act
            var response = await httpclient.GetAsync($"api/auctions/notAGUID");

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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