using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;
using System;
using System.Threading.Tasks;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper mapper;

        public AuctionCreatedConsumer(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("--> consuming auction created: " + context.Message.Id);
            var item = mapper.Map<Item>(context.Message);
            if (item.Model == "Foo") throw new ArgumentException("Not foo allowed");
            await item.SaveAsync();
        }
    }
}
