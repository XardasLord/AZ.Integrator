using AZ.Integrator.Application.Common.ExternalServices.ShipX;

namespace AZ.Integrator.Infrastructure.ExternalServices.ShipX;

public class ShipXApiService : IShipXService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ShipXApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
}