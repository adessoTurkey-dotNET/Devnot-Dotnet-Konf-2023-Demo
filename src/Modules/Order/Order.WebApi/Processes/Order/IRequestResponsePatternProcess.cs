using Order.WebApi.Processes.Order.Request;

namespace Order.WebApi.Processes.Order;

public interface IRequestResponsePatternProcess
{
    public Task ProcessAsync(RequestResponsePatternProcessRequest request);
}