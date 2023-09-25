namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public class OrderListDto
{
    public Guid OrderId { get; set; }
    public DateTime Date { get; set; }
    public BuyerDto Buyer { get; set; }
    public List<LineItemDto> LineItems { get; set; }
}

public class BuyerDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public bool Guest { get; set; }
}

public class LineItemDto
{
    public string Id { get; set; }
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public PriceDto OriginalPrice { get; set; }
    public PriceDto Price { get; set; }
    public DateTime BoughtAt { get; set; }
}

public class PriceDto
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}