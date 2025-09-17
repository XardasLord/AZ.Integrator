using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia.Models;

public sealed class InvoiceDataStatusEnum : SmartEnum<InvoiceDataStatusEnum>
{
    public static readonly InvoiceDataStatusEnum Issued = new("issued", 0);
    public static readonly InvoiceDataStatusEnum Sent = new("sent", 1);
    public static readonly InvoiceDataStatusEnum Paid = new("paid", 2);
    public static readonly InvoiceDataStatusEnum Partial = new("partial", 3);
    public static readonly InvoiceDataStatusEnum Rejected = new("rejected", 3);
    
    private InvoiceDataStatusEnum(string name, int value) : base(name, value)
    {
    }
}