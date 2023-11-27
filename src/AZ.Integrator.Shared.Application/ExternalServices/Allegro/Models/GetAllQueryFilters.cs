namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class GetAllQueryFilters
{
    public IEnumerable<string> OrderFulfillmentStatus { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
    public string SearchText { get; set; }
}