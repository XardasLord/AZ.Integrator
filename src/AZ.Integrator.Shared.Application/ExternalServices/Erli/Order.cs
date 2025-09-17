namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public class Order
{
    public string Id { get; set; }
    public string Status { get; set; }
    public User User { get; set; }
    public List<Item> Items { get; set; }
    public Delivery Delivery { get; set; }
    public string Comment { get; set; }
    public int TotalPrice { get; set; }
    public DeliveryTracking DeliveryTracking { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime PurchasedAt { get; set; }
    public Payment Payment { get; set; }
    public string SellerStatus { get; set; }
}

public class User
{
    public string Email { get; set; }
    public DeliveryAddress DeliveryAddress { get; set; }
    public InvoiceAddress InvoiceAddress { get; set; }
}

public class DeliveryAddress
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string FlatNumber { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
}

public class Item
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public int Quantity { get; set; }
    public int Weight { get; set; }
    public int UnitPrice { get; set; } // W groszach
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Ean { get; set; }
    public string Sku { get; set; }
}

public class Delivery
{
    public string Name { get; set; }
    public string TypeId { get; set; }
    public int Price { get; set; }
    public bool Cod { get; set; }
}

public class DeliveryTracking
{
    public string Vendor { get; set; }
    public string TrackingNumber { get; set; }
    public string TrackingUrl { get; set; }
    public string Status { get; set; }
}

public class Payment
{
    public int Id { get; set; }
}

public class InvoiceAddress
{
    public string Type { get; set; }
    public string Address { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string FlatNumber { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string Nip { get; set; }
}