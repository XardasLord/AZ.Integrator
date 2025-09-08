using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Fakturownia.Models;

public sealed class InvoiceDataPaymentTypeEnum : SmartEnum<InvoiceDataPaymentTypeEnum>
{
    public static readonly InvoiceDataPaymentTypeEnum Online = new("transfer", 0);
    public static readonly InvoiceDataPaymentTypeEnum Cod = new("cash_on_delivery", 1);
    public static readonly InvoiceDataPaymentTypeEnum Cash = new("cash", 2);
    
    private InvoiceDataPaymentTypeEnum(string name, int value) : base(name, value)
    {
    }
}