using System.Runtime.CompilerServices;
using MassTransit;

namespace OrderStateMachine.Domain.Events;

public class OrderReceived
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }
    
    [ModuleInitializer]
    internal static  void Init()
    {
        GlobalTopology.Send.UseCorrelationId<OrderReceived>(x=> x.OrderId);
    }
}
