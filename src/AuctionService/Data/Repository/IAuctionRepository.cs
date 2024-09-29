using AuctionService.DTOs;
using AuctionService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionService.Data.Repository
{
    public interface IAuctionRepository
    {
        Task<List<AuctionDto>> GetAuctionsAsync(string date);
        Task<AuctionDto> GetAuctionByIdAsync(Guid id);
        Task<Auction> GetAuctionEntityById(Guid id);
        void AddAuction(Auction auction);
        void RemoveAuction(Auction auction);
        Task<bool>SaveChangesAsync();
    }
}
