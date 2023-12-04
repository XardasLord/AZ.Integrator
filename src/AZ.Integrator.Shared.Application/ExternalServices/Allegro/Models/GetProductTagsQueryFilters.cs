namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class GetProductTagsQueryFilters
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public string SearchText { get; set; }
}