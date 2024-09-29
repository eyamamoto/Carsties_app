using AuctionService.Entities;

namespace AuctionService
{
    public class AuctionEntityTests
    {
        [Fact]
        public void HasReservePrice_ReservePriceGreatherZero_True()
        {
            //Arrange
            var auction = new Auction { Id= Guid.NewGuid(), ReservePrice = 10};

            //act
            var result = auction.HasReservePrice();

            //assert
            Assert.True(result);
            Assert.Equal(10, auction.ReservePrice);
        }

        [Fact]
        public void HasReservePrice_ReservePriceIsZero_True()
        {
            //Arrange
            var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 0 };

            //act
            var result = auction.HasReservePrice();

            //assert
            Assert.False(result);
            Assert.Equal(0, auction.ReservePrice);
        }
    }
}