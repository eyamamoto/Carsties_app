using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;
using System.Threading.Tasks;

namespace SearchService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFInished>
    {
        public async Task Consume(ConsumeContext<AuctionFInished> context)
        {
            var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = (int)context.Message.Amount;
            }

            auction.Status = "Finished";

            await auction.SaveAsync();
        }
    }
}
