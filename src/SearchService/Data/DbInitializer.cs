using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            //mongodb
            await DB.InitAsync("SearchDb",
                MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
            //cria o index da entidade
            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
            var items = await httpClient.GetItemsForSearchDb();

            Console.WriteLine(items.Count + " returned from the auction service");
            
            if(items.Count > 0) await DB.SaveAsync(items);

            //if(count == 0)
            //{
            //    Console.WriteLine("no data will attempt to seed");

            //    var itemData = await File.ReadAllTextAsync("Data/auctions.json");
            //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //    var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
                
            //    await DB.SaveAsync(items);
            //}
        }
    }
}
