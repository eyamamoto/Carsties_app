using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using System;
using System.Threading.Tasks;

namespace NotificationService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("COnsume => auction created message received !");

            await hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
        }
    }
}
