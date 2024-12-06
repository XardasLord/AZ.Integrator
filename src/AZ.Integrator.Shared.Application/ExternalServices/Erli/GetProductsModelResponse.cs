namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public class GetProductsModelResponse
{
    public IEnumerable<Product> Products { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}