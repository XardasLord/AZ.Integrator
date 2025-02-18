using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public sealed class ErliOrderSellerStatusEnum : SmartEnum<ErliOrderSellerStatusEnum>
{
    public static readonly ErliOrderSellerStatusEnum New = new("readyToProcess", 0);
    public static readonly ErliOrderSellerStatusEnum ReadyToProcess = new("readyToProcess", 1);
    public static readonly ErliOrderSellerStatusEnum Sent = new("sent", 2);
    
    private ErliOrderSellerStatusEnum(string name, int value) : base(name, value)
    {
    }
}
