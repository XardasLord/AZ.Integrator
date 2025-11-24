namespace AZ.Integrator.Integrations.Domain.Aggregates.Inpost.ValueObjects;

public class SenderData
{
    public string Name { get; private set; }
    public string CompanyName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public SenderDataAddress Address { get; private set; }
    
    private SenderData() { }

    public SenderData(
        string name,
        string companyName,
        string firstName,
        string lastName,
        string email,
        string phone,
        SenderDataAddress address) : this()
    {
        Name = name;
        CompanyName = companyName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Address = address;
    }
}