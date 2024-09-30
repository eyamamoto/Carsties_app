using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared collection")]
    public class AuctionControllerIntegrationTests : IAsyncLifetime
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

        [Fact]
        public async Task CreateAuction_WithNoAuth_ShouldReturn401()
        {
            //arrange
            var auction = new CreateAuctionDto { Make = "test" };

            //act
            var response = await httpclient.PostAsJsonAsync($"api/auctions", auction);

            //assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task CreateAuction_WithAuth_ShouldReturn201()
        {
            // arrange
            var auction = GetAuctionForCreate();
            httpclient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await httpclient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
            Assert.Equal("bob", createdAuction?.Seller);
        }

        [Fact]
        public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
        {
            // arrange
            var auction = GetAuctionForCreate();
            auction.Make = null!;
            httpclient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await httpclient.PostAsJsonAsync($"api/auctions", auction);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
        {
            // arrange
            var updatedAuction = new UpdateAuctionDto { Make = "Updated" };
            httpclient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await httpclient.PutAsJsonAsync($"api/auctions/{Model_T_ID}", updatedAuction);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
        {
            // arrange
            var updatedAuction = new UpdateAuctionDto { Make = "Updated" };
            httpclient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("notbob"));

            // act
            var response = await httpclient.PutAsJsonAsync($"api/auctions/{Model_T_ID}", updatedAuction);

            // assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
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