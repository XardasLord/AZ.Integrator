namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public class GetOffersResponse
{
    public IEnumerable<OfferListDetails> Offers { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}

public class OfferListDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
    public External External { get; set; }
}