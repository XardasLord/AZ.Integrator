namespace AZ.Integrator.Shipments.Application.Common.Configurations;

public class InpostSenderOptions
{
    public SenderData SenderData { get; set; }
}

public class SenderData
{
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public SenderDataAddress Address { get; set; }
}

public class SenderDataAddress
{
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string CountryCode { get; set; }
}