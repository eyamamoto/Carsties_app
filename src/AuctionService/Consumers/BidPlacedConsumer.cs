﻿using AuctionService.Data;
using Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _context;

        public BidPlacedConsumer(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("---> consuming bid placed");
            var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if(auction.CurrentHighBid == null 
                || context.Message.BidStatus.Contains("Accepted") 
                && context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _context.SaveChangesAsync();
            }
        }
    }
}
