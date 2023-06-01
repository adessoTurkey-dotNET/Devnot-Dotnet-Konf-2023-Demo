using EventConfiguration.Base;
using MassTransit;
using OrderStateMachine.Domain.Events;
using OrderStateMachine.Domain.Responses;

namespace Product.WebApi.Consumers;

public class ProductReservedConsumer : ConsumerBase<ProductReserved>
{
    protected override async Task ConsumeInternal(ConsumeContext<ProductReserved> context)
    {
        //from database.
        var productName = "Iphone";

        // await context.Publish(new SagaProductInfoUpdated()
        // {
        //     OrderId = context.Message.OrderId,
        //     ProductName = productName
        // });
        //
        // //to imitate real life example.
        await Task.Delay(100);
        
        if (context.Message.ProductQuantity > 10)
        {
            throw new Exception("Amount should not be more than 10");
        }

        Console.WriteLine("Product reserved.");
        
        await context.RespondAsync(new ProductReservedEventDto()
        {
            OrderId = context.Message.OrderId
        });
    }
}