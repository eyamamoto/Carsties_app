using AuctionService.Data.Repository;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace AuctionService.Services
{
    public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
    {
        private readonly IAuctionRepository repository;

        public GrpcAuctionService(IAuctionRepository repository)
        {
            this.repository = repository;
        }

        public override async Task<GrpcAuctionResponse>GetAuction(GetAuctionRequest request,ServerCallContext context)
        {
            Console.WriteLine("==> received GRPC request for auction");

            var auctions = await repository.GetAuctionEntityById(Guid.Parse(request.Id));

            if (auctions == null) throw new RpcException(new Status(StatusCode.NotFound, "Notfound"));

            var response = new GrpcAuctionResponse
            {
                Auction = new GrpcAuctionModel
                {
                    AuctionEnd = auctions.AuctionEnd.ToString(),
                    Id = auctions.Id.ToString(),
                    ReservePrice = auctions.ReservePrice,
                    Seller = auctions.Seller
                }
            };

            return response;
        }
    }
}
