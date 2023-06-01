namespace Order.WebApi.Requests;

public class CreateOrderRequest
{
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }
}