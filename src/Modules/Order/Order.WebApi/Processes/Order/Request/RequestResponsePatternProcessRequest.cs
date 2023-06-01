namespace Order.WebApi.Processes.Order.Request;

public class RequestResponsePatternProcessRequest
{
    public RequestResponsePatternProcessRequest(Guid productId, int productQuantity)
    {
        ProductId = productId;
        ProductQuantity = productQuantity;
    }
    public Guid ProductId { get; }
    public int ProductQuantity { get; }
}