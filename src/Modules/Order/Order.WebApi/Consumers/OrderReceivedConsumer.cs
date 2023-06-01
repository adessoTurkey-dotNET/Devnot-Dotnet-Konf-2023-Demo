using EventConfiguration.Base;
using MassTransit;
using OrderStateMachine.Domain.Events;
using OrderStateMachine.Domain.Responses;

namespace Order.WebApi.Consumers;

public class OrderReceivedConsumer : ConsumerBase<OrderReceived>
{
    protected override async Task ConsumeInternal(ConsumeContext<OrderReceived> context)
    {
        Console.WriteLine("Order Received checked.");

        //to imitate real life example.
        await Task.Delay(100);
        
        await context.RespondAsync(new OrderReceivedEventDto()
        {
            OrderId = context.Message.OrderId
        });
    }
}