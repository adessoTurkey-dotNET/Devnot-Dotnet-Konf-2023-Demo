using System.Runtime.CompilerServices;
using MassTransit;

namespace OrderStateMachine.Domain.Events;

public class SagaPaymentInfoUpdated
{
    public Guid OrderId { get; set; }
    public int ReceiptId { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<SagaPaymentInfoUpdated>(x => x.OrderId);
    }
}