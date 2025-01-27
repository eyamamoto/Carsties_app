﻿using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Data.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext context;
        private readonly IMapper mapper;

        public AuctionRepository(AuctionDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public void AddAuction(Auction auction)
        {
            context.Auctions.Add(auction);
        }

        public async Task<AuctionDto> GetAuctionByIdAsync(Guid id)
        {
            return await context.Auctions
               .ProjectTo<AuctionDto>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Auction> GetAuctionEntityById(Guid id)
        {
            return await context.Auctions.Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AuctionDto>> GetAuctionsAsync(string date)
        {
            var query = context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            return await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveAuction(Auction auction)
        {
            context.Auctions.Remove(auction);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
