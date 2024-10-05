using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using System;
using System.Threading.Tasks;

namespace NotificationService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFInished>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<AuctionFInished> context)
        {
            Console.WriteLine("COnsume => auction finished message received !");

            await hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
        }
    }
}
