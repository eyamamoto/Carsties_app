using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
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

            if(count == 0)
            {
                Console.WriteLine("no data will attempt to seed");

                var itemData = await File.ReadAllTextAsync("Data/auctions.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
                
                await DB.SaveAsync(items);
            }
        }
    }
}
