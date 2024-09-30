using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Contracts;
using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.IntegrationTests
{
    public class AuctionBusTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory factory;
        private readonly HttpClient httpclient;
        //in memory service bus
        private readonly ITestHarness testHarness;

        public AuctionBusTests(CustomWebAppFactory factory)
        {
            this.factory = factory;
            httpclient = factory.CreateClient();
            testHarness = factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
        {
            //arrange
            var auction = GetAuctionForCreate();
            httpclient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            //act
            var response = await httpclient.PostAsJsonAsync("api/auctions", auction);

            //assert
            response.EnsureSuccessStatusCode();
            Assert.True(await testHarness.Published.Any<AuctionCreated>());
        }


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

        private CreateAuctionDto GetAuctionForCreate()
        {
            return new CreateAuctionDto
            {
                Make = "test",
                Model = "testModel",
                ImageUrl = "test",
                Color = "test",
                Mileage = 10,
                Year = 10,
                ReservePrice = 10,
            };
        }
    }
}
