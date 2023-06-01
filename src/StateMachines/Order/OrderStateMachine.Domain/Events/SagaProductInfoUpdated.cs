using System.Runtime.CompilerServices;
using MassTransit;

namespace OrderStateMachine.Domain.Events;

public class SagaProductInfoUpdated
{
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<SagaProductInfoUpdated>(x => x.OrderId);
    }
}