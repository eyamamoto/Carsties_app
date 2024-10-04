using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiddingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly GrpcAuctionClient grpcClient;

        public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcClient)
        {
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            this.grpcClient = grpcClient;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BidDto>>PlaceBid(string auctionId, int amount)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);

            if(auction == null)
            {
                //TODO check with auctionService if has auction with GRPC
                auction = grpcClient.GetAuction(auctionId);
                if (auction == null) return BadRequest("error in placebid grpc");
            }

            if(auction.Seller == User.Identity.Name)
            {
                return BadRequest("You cannot bid on your own auction");
            }

            var bid = new Bid
            {
                Amount = amount,
                AuctionId = auctionId,
                Bidder = User.Identity.Name
            };

            if(auction.AuctionEnd < DateTime.UtcNow)
            {
                bid.BidStatus = BidStatus.Finished;
            }
            else
            {
                var highBid = await DB.Find<Bid>()
                .Match(a => a.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();

                if (highBid != null && amount > highBid.Amount || highBid == null)
                {
                    bid.BidStatus = amount > auction.ReservePrice
                        ? BidStatus.Accepted
                        : BidStatus.AcceptedBelowReserve;
                }

                if (highBid != null && bid.Amount <= highBid.Amount)
                {
                    bid.BidStatus = BidStatus.TooLow;
                }
            }

            await DB.SaveAsync(bid);

            //depois de salvar o lance, publica um evento no rabbitmq
            await publishEndpoint.Publish(mapper.Map<BidPlaced>(bid));

            return Ok(mapper.Map<BidDto>(bid));
        }

        [HttpGet("{auctionId}")]
        public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
        {
            var bids = await DB.Find<Bid>()
                .Match(a => a.AuctionId == auctionId)
                .Sort(b => b.Ascending(a => a.BidTime))
                .ExecuteAsync();

            return bids.Select(mapper.Map<BidDto>).ToList();
        }
    }
}
