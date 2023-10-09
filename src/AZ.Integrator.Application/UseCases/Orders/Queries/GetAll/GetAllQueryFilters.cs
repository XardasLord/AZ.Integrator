namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

public class GetAllQueryFilters
{
    public string OrderFulfillmentStatus { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
}