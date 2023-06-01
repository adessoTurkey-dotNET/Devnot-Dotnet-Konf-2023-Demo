using EventConfiguration.Base;
using MassTransit;
using OrderStateMachine.Domain.Events;
using OrderStateMachine.Domain.Responses;

namespace Order.WebApi.Consumers;

public class OrderCompletedConsumer : ConsumerBase<OrderCompleted>
{
    protected override async Task ConsumeInternal(ConsumeContext<OrderCompleted> context)
    {
        Console.WriteLine("Order Received checked.");
        
        //to imitate real life example.
        await Task.Delay(100);
        
        await context.RespondAsync(new OrderCompletedEventDto
        {
            OrderId = context.Message.OrderId
        });
    }
}