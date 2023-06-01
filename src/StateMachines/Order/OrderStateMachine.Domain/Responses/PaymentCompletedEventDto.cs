namespace OrderStateMachine.Domain.Responses;

public class PaymentCompletedEventDto
{
    public Guid OrderId { get; set; }
}