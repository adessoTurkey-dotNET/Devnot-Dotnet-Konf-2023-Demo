using System.Runtime.CompilerServices;
using MassTransit;

namespace OrderStateMachine.Domain.Events;

public class OrderCompleted
{
    public Guid OrderId { get; set; }
    
    [ModuleInitializer]
    internal static  void Init()
    {
        GlobalTopology.Send.UseCorrelationId<OrderCompleted>(x=> x.OrderId);
    }
}