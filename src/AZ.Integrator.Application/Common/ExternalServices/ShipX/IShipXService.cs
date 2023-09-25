using AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;

namespace AZ.Integrator.Application.Common.ExternalServices.ShipX;

public interface IShipXService
{
    // https://dokumentacja-inpost.atlassian.net/wiki/spaces/PL/pages/40206337/Proces+integracji+z+us+ugami+InPost
    // 1. Utworzenie przesyłki
    // 2. Sprawdzenie statusu przesyłki
    // 3. Pobranie etykiety
    // 4. Zamówienie kuriera (nie jest potrzebne, ponieważ firma AZ posiada stałe zlecenie odbioru)
    // 5. Wygenerowanie potwierdzenia nadania
    // 6. Tracking przesyłki (zgodnie z ustaleniami w MVP nie będzie potrzebne śledzenie przesyłek)

    Task<ShipmentResponse> CreateShipment(Shipment shipment);
}