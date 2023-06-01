using MassTransit;
using Order.WebApi.Processes.Order.Request;
using OrderStateMachine.Domain.Events;
using OrderStateMachine.Domain.Responses;

namespace Order.WebApi.Processes.Order;

public class RequestResponsePatternProcess : IRequestResponsePatternProcess
{
    private readonly IRequestClient<OrderReceived> _orderReceivedRequestClient;
    private readonly IRequestClient<ProductReserved> _productReservedRequestClient;
    private readonly IRequestClient<PaymentCompleted> _paymentCompletedRequestClient;
    private readonly IRequestClient<OrderCompleted> _orderCompletedRequestClient;

    public RequestResponsePatternProcess(IRequestClient<OrderReceived> orderReceivedRequestClient,
        IRequestClient<ProductReserved> productReservedRequestClient,
        IRequestClient<PaymentCompleted> paymentCompletedRequestClient,
        IRequestClient<OrderCompleted> orderCompletedRequestClient)
    {
        _orderReceivedRequestClient = orderReceivedRequestClient;
        _productReservedRequestClient = productReservedRequestClient;
        _paymentCompletedRequestClient = paymentCompletedRequestClient;
        _orderCompletedRequestClient = orderCompletedRequestClient;
    }


    public async Task ProcessAsync(RequestResponsePatternProcessRequest request)
    {
        try
        {
            var orderId = Guid.NewGuid();

            var orderReceived = await _orderReceivedRequestClient.GetResponse<OrderReceivedEventDto>(new OrderReceived
            {
                OrderId = orderId,
                ProductId = request.ProductId,
                ProductQuantity = request.ProductQuantity
            });

            var productReserved = await _productReservedRequestClient.GetResponse<ProductReservedEventDto>(
                new ProductReserved
                {
                    OrderId = orderId,
                    ProductId = request.ProductId,
                    ProductQuantity = request.ProductQuantity
                });

            var paymentCompleted = await _paymentCompletedRequestClient.GetResponse<PaymentCompletedEventDto>(
                new PaymentCompleted
                {
                    OrderId = orderId,
                });

            var orderCompleted = await _orderCompletedRequestClient.GetResponse<OrderCompletedEventDto>(new OrderCompleted
            {
                OrderId = orderId,
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}