namespace AZ.Integrator.Application.Common.ExternalServices.ShipX.Models;

using System.Collections.Generic;

public class Shipment
{
    public Sender Sender { get; set; }
    public Receiver Receiver { get; set; }
    public List<Parcel> Parcels { get; set; }
    public Insurance Insurance { get; set; }
    public Cod Cod { get; set; }
    public string Service { get; set; }
    public List<string> AdditionalServices { get; set; }
    public string Reference { get; set; }
    public string Comments { get; set; }
    public string ExternalCustomerId { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string CountryCode { get; set; }
}

public class Sender
{
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address Address { get; set; }
}

public class Receiver
{
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address Address { get; set; }
}

public class Dimensions
{
    public string Length { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string Unit { get; set; }
}

public class Weight
{
    public string Amount { get; set; }
    public string Unit { get; set; }
}

public class Parcel
{
    public string Id { get; set; }
    public Dimensions Dimensions { get; set; }
    public Weight Weight { get; set; }
    public bool IsNonStandard { get; set; }
}

public class Insurance
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

public class Cod
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

