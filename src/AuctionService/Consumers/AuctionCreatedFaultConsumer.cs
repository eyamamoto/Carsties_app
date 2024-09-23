using Contracts;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            Console.WriteLine("-----------> consuming fault creation");
            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                Console.WriteLine("-----------> consuming fault creation ok");
                context.Message.Message.Model = "FooBar";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("-----------> not argument exception");
            }
        }
    }
}
