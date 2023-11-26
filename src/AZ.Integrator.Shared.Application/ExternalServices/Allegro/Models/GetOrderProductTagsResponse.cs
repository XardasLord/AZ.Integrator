namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class GetOrderProductTagsResponse
{
    public IEnumerable<ProductTag> Tags { get; set; }
}

public class ProductTag
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Hidden { get; set; }
}