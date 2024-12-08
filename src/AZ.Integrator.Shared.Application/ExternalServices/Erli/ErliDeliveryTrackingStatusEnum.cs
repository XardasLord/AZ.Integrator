using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public sealed class ErliDeliveryTrackingStatusEnum : SmartEnum<ErliDeliveryTrackingStatusEnum>
{
    public static readonly ErliDeliveryTrackingStatusEnum Preparing = new("preparing", 0);
    public static readonly ErliDeliveryTrackingStatusEnum ReadyToSend = new("readyToSend", 1);
    public static readonly ErliDeliveryTrackingStatusEnum WaitingForCourier = new("waitingForCourier", 2);
    public static readonly ErliDeliveryTrackingStatusEnum Sent = new("sent", 3);
    
    private ErliDeliveryTrackingStatusEnum(string name, int value) : base(name, value)
    {
    }
}
