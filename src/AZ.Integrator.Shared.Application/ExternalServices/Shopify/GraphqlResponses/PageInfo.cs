namespace AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

public class PageInfo
{
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public string StartCursor { get; set; }
    public string EndCursor { get; set; }
}