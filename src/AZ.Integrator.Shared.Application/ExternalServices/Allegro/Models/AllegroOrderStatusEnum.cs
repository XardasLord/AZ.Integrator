using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

public sealed class AllegroFulfillmentStatusEnum : SmartEnum<AllegroFulfillmentStatusEnum>
{
    public static readonly AllegroFulfillmentStatusEnum New = new("NEW", 0);
    public static readonly AllegroFulfillmentStatusEnum Processing = new("PROCESSING", 1);
    public static readonly AllegroFulfillmentStatusEnum ReadyForShipment = new("READY_FOR_SHIPMENT", 2);
    public static readonly AllegroFulfillmentStatusEnum ReadyForPickup = new("READY_FOR_PICKUP", 3);
    public static readonly AllegroFulfillmentStatusEnum Sent = new("SENT", 4);
    public static readonly AllegroFulfillmentStatusEnum PickedUp = new("PICKED_UP", 5);
    public static readonly AllegroFulfillmentStatusEnum Cancelled = new("CANCELLED", 6);
    public static readonly AllegroFulfillmentStatusEnum Suspended = new("SUSPENDED", 7);
    
    private AllegroFulfillmentStatusEnum(string name, int value) : base(name, value)
    {
    }
}
