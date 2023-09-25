namespace AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;

public class ShipmentResponse
{
    public string Href { get; set; }
    public int Id { get; set; }
    public string Status { get; set; }
    public object TrackingNumber { get; set; }
    public string Service { get; set; }
    public string Reference { get; set; }
    public bool IsReturn { get; set; }
    public int ApplicationId { get; set; }
    public object CreatedById { get; set; }
    public string ExternalCustomerId { get; set; }
    public object SendingMethod { get; set; }
    public Mpk Mpk { get; set; }
    public string Comments { get; set; }
    public List<string> AdditionalServices { get; set; }
    public Dictionary<string, object> CustomAttributes { get; set; }
    public CodResponse Cod { get; set; }
    public InsuranceResponse Insurance { get; set; }
    public SenderResponse Sender { get; set; }
    public ReceiverResponse Receiver { get; set; }
    public object SelectedOffer { get; set; }
    public List<object> Offers { get; set; }
    public List<object> Transactions { get; set; }
    public List<ParcelResponse> Parcels { get; set; }
}
public class AddressResponse
{
    public string Id { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string CountryCode { get; set; }
}

public class Mpk
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CodResponse
{
    public double Amount { get; set; }
    public string Currency { get; set; }
}

public class InsuranceResponse
{
    public double Amount { get; set; }
    public string Currency { get; set; }
}

public class WeightResponse
{
    public double Amount { get; set; }
    public string Unit { get; set; }
}

public class DimensionsResponse
{
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string Unit { get; set; }
}

public class ParcelResponse
{
    public int Id { get; set; }
    public object TrackingNumber { get; set; }
    public bool IsNonStandard { get; set; }
    public object Template { get; set; }
    public DimensionsResponse Dimensions { get; set; }
    public WeightResponse Weight { get; set; }
}

public class SenderResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressResponse Address { get; set; }
}

public class ReceiverResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressResponse Address { get; set; }
}