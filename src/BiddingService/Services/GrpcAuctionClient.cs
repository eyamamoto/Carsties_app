﻿
using AuctionService;
using BiddingService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace BiddingService.Services
{
    public class GrpcAuctionClient
    {
        private readonly ILogger<GrpcAuctionClient> logger;
        private readonly IConfiguration config;

        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger,IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        public Auction GetAuction(string id)
        {
            logger.LogInformation("Calling GRPC Server");

            var channel = GrpcChannel.ForAddress(config["GrpcAuction"]);
            var client = new GrpcAuction.GrpcAuctionClient(channel);
            var request = new GetAuctionRequest { Id = id };

            try
            {
                var reply = client.GetAuction(request);
                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice,
                };

                return auction;
            }catch(System.Exception ex)
            {
                logger.LogError(ex, "erro no grpc bidding service");
                return null;
            }
        }
    }
}
