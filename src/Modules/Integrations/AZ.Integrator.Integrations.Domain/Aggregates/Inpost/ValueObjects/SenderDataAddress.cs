namespace AZ.Integrator.Integrations.Domain.Aggregates.Inpost.ValueObjects;

public class SenderDataAddress
{
    public string Street { get; private set; }
    public string BuildingNumber { get; private set; }
    public string City { get; private set; }
    public string PostCode { get; private set; }
    public string CountryCode { get; private set; }
    
    private SenderDataAddress() { }

    public SenderDataAddress(
        string street,
        string buildingNumber,
        string city,
        string postCode,
        string countryCode) : this()
    {
        Street = street;
        BuildingNumber = buildingNumber;
        City = city;
        PostCode = postCode;
        CountryCode = countryCode;
    }
}