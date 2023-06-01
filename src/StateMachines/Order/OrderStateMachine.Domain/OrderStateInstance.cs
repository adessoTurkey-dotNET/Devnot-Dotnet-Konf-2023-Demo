using MassTransit;

namespace OrderStateMachine.Domain;

public class OrderStateInstance : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int ReceiptId { get; set; }
    
    public DateTime OrderStartDate { get; set; }
    public DateTime OrderEndDate { get; set; }
    public int Version { get; set; }
}