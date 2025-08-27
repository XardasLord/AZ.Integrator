using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

public sealed class CurrencyEnum : SmartEnum<CurrencyEnum>
{
    public static readonly CurrencyEnum Pln = new("PLN", 0);
    
    private CurrencyEnum(string name, int value) : base(name, value)
    {
    }
}