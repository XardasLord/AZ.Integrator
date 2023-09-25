using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.Common.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<IEnumerable<OrderEvent>> GetOrderEvents();
}