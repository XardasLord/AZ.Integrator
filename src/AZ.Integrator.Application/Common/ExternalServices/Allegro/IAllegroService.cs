namespace AZ.Integrator.Application.Common.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<object> GetOrders();
}