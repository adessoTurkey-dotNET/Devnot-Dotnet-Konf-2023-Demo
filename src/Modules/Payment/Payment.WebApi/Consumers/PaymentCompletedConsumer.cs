using EventConfiguration.Base;
using MassTransit;
using OrderStateMachine.Domain.Events;
using OrderStateMachine.Domain.Responses;

namespace Payment.WebApi.Consumers;

public class PaymentCompletedConsumer : ConsumerBase<PaymentCompleted>
{
    protected override async Task ConsumeInternal(ConsumeContext<PaymentCompleted> context)
    {
        Console.WriteLine("Payment Completed.");

        //from database.
        var receiptId = 52;

        await context.Publish(new SagaPaymentInfoUpdated
        {
            OrderId = context.Message.OrderId,
            ReceiptId = receiptId
        });

        //to imitate real life example.
        await Task.Delay(500);

        await context.RespondAsync(new PaymentCompletedEventDto
        {
            OrderId = context.Message.OrderId
        });

    }
}