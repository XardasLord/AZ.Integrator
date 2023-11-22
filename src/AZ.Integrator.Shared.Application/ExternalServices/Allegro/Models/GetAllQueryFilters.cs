namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class GetAllQueryFilters
{
    public string OrderFulfillmentStatus { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
}