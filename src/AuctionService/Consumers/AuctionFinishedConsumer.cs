﻿using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AuctionService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFInished>
    {
        private readonly AuctionDbContext _context;

        public AuctionFinishedConsumer(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<AuctionFInished> context)
        {
            Console.WriteLine("---> consumingAuction Finished");

            var auction = await _context.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            auction.Status = auction.SoldAmount > auction.ReservePrice
                ? Status.Finished : Status.ReserveNotMet;

            await _context.SaveChangesAsync();
        }
    }
}
