namespace AZ.Integrator.Infrastructure.ExternalServices.Dpd;

public class DpdOptions
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int MasterFid { get; set; }
    public DpdSender Sender { get; set; }
}

public class DpdSender
{
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Company { get; set; }
    public string CountryCode { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string CodAccountNumber { get; set; }
}