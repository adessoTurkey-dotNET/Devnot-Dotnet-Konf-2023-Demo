namespace Order.WebApi.Responses;

public class CreateOrderDto
{
    public CreateOrderDto(bool isOrderProcessStarted)
    {
        IsOrderProcessStarted = isOrderProcessStarted;
    }
    public bool IsOrderProcessStarted { get; }
}