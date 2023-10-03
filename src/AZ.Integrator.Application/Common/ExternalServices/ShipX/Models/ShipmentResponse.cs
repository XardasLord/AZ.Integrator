using System.Text.Json.Serialization;

namespace AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;

public class ShipmentResponse
{
    [JsonPropertyName("href")] 
    public string Href { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("tracking_number")]
    public object TrackingNumber { get; set; }

    [JsonPropertyName("service")]
    public string Service { get; set; }

    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("is_return")]
    public bool IsReturn { get; set; }

    [JsonPropertyName("application_id")]
    public int ApplicationId { get; set; }

    [JsonPropertyName("created_by_id")]
    public object CreatedById { get; set; }

    [JsonPropertyName("external_customer_id")]
    public string ExternalCustomerId { get; set; }

    [JsonPropertyName("sending_method")]
    public object SendingMethod { get; set; }

    [JsonPropertyName("end_of_week_collection")]
    public bool EndOfWeekCollection { get; set; }

    [JsonPropertyName("mpk")]
    public Mpk Mpk { get; set; }
    
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    
    [JsonPropertyName("additional_services")]
    public List<string> AdditionalServices { get; set; }
    
    [JsonPropertyName("custom_attributes")]
    public Dictionary<string, object> CustomAttributes { get; set; }
    
    [JsonPropertyName("cod")]
    public CodResponse Cod { get; set; }
    
    [JsonPropertyName("insurance")]
    public InsuranceResponse Insurance { get; set; }
    
    [JsonPropertyName("sender")]
    public SenderResponse Sender { get; set; }
    
    [JsonPropertyName("receiver")]
    public ReceiverResponse Receiver { get; set; }
    
    [JsonPropertyName("selected_offer")]
    public object SelectedOffer { get; set; }
    
    [JsonPropertyName("offers")]
    public List<object> Offers { get; set; }
    
    [JsonPropertyName("transactions")]
    public List<object> Transactions { get; set; }
    
    [JsonPropertyName("parcels")]
    public List<ParcelResponse> Parcels { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
public class AddressResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("building_number")]
    public string BuildingNumber { get; set; }

    [JsonPropertyName("line1")]
    public string Line1 { get; set; }

    [JsonPropertyName("line2")]
    public string Line2 { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }
}

public class Mpk
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class CodResponse
{
    [JsonPropertyName("amount")]
    public double? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

public class InsuranceResponse
{
    [JsonPropertyName("amount")]
    public double? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

public class WeightResponse
{
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    
    [JsonPropertyName("unit")]
    public string Unit { get; set; }
}

public class DimensionsResponse
{
    [JsonPropertyName("length")]
    public double Length { get; set; }
    
    [JsonPropertyName("width")]
    public double Width { get; set; }
    
    [JsonPropertyName("height")]
    public double Height { get; set; }
    
    [JsonPropertyName("unit")]
    public string Unit { get; set; }
}

public class ParcelResponse
{
    [JsonPropertyName("unit")]
    public int Id { get; set; }
    
    [JsonPropertyName("identify_number")]
    public string IdentifyNumber { get; set; }
    
    [JsonPropertyName("tracking_number")]
    public object TrackingNumber { get; set; }
    
    [JsonPropertyName("is_non_standard")]
    public bool IsNonStandard { get; set; }
    
    [JsonPropertyName("template")]
    public object Template { get; set; }
    
    [JsonPropertyName("dimensions")]
    public DimensionsResponse Dimensions { get; set; }
    
    [JsonPropertyName("weight")]
    public WeightResponse Weight { get; set; }
}

public class SenderResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
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
    public AddressResponse Address { get; set; }
}

public class ReceiverResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
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
    public AddressResponse Address { get; set; }
}