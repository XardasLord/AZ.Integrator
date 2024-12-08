using System.Text.Json.Serialization;

namespace AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;

public class Shipment
{
    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }
    
    [JsonPropertyName("receiver")]
    public Receiver Receiver { get; set; } // REQ
    
    [JsonPropertyName("parcels")] // REQ
    public List<Parcel> Parcels { get; set; }
    
    [JsonPropertyName("insurance")]
    public Insurance Insurance { get; set; }
    
    [JsonPropertyName("cod")]
    public Cod Cod { get; set; }
    
    [JsonPropertyName("service")] // REQ
    public string Service { get; set; }

    [JsonPropertyName("additional_services")]
    public List<string> AdditionalServices { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("comments")]
    public string Comments { get; set; }

    [JsonPropertyName("external_customer_id")]
    public string ExternalCustomerId { get; set; }
}

public class Address // REQ
{
    [JsonPropertyName("street")]
    public string Street { get; set; }
    
    [JsonPropertyName("building_number")]
    public string BuildingNumber { get; set; }
    
    [JsonPropertyName("city")]
    public string City { get; set; }
    
    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }
    
    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }
}

public class Sender
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    
    [JsonPropertyName("address")]
    public Address Address { get; set; }
}

public class Receiver
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    
    [JsonPropertyName("address")]
    public Address Address { get; set; }
}

public class Dimensions
{
    [JsonPropertyName("length")]
    public string Length { get; set; }
    
    [JsonPropertyName("width")]
    public string Width { get; set; }
    
    [JsonPropertyName("height")]
    public string Height { get; set; }
    
    [JsonPropertyName("unit")]
    public string Unit { get; set; }
}

public class Weight
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }
    
    [JsonPropertyName("unit")]
    public string Unit { get; set; }
}

public class Parcel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("dimensions")]
    public Dimensions Dimensions { get; set; }
    
    [JsonPropertyName("weight")]
    public Weight Weight { get; set; }
    
    [JsonPropertyName("is_non_standard")]
    public bool IsNonStandard { get; set; }
}

public class Insurance
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

public class Cod
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

