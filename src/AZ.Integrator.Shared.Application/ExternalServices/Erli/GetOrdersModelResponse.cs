namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public class GetOrdersModelResponse
{
    public IEnumerable<Order> Orders { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}