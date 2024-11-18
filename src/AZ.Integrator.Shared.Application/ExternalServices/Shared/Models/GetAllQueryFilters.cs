namespace AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

public class GetAllQueryFilters
{
    // TODO: ChangeName to be common with Erli and other shops in the future
    public IEnumerable<string> OrderFulfillmentStatus { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
    public string SearchText { get; set; }
}