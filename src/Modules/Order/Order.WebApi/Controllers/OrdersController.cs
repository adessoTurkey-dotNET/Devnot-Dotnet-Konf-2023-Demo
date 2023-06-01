using Microsoft.AspNetCore.Mvc;
using Order.WebApi.Processes.Order;
using Order.WebApi.Processes.Order.Request;
using Order.WebApi.Requests;
using Order.WebApi.Responses;

namespace Order.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IRequestResponsePatternProcess _requestResponsePatternProcess;

    public OrdersController(IRequestResponsePatternProcess requestResponsePatternProcess)
    {
        _requestResponsePatternProcess = requestResponsePatternProcess;
    }

    /// <summary>
    /// Create order.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CreateOrderDto> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var processRequest = new RequestResponsePatternProcessRequest(request.ProductId, request.ProductQuantity);
        
        await _requestResponsePatternProcess.ProcessAsync(processRequest);
        
        return new CreateOrderDto(true);
    }
}