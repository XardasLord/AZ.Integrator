namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public class OrderListDto
{
    public Guid OrderId { get; set; }
    public DateTime Date { get; set; }
    public BuyerDto Buyer { get; set; }
}

public class BuyerDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public bool Guest { get; set; }
}