using System.Runtime.CompilerServices;
using MassTransit;

namespace OrderStateMachine.Domain.Events;

public class PaymentCompleted
{
    public Guid OrderId { get; set; }
    
    [ModuleInitializer]
    internal static  void Init()
    {
        GlobalTopology.Send.UseCorrelationId<PaymentCompleted>(x=> x.OrderId);
    }
}