using Ardalis.SmartEnum;

namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public sealed class AllegroStatusEnum : SmartEnum<AllegroStatusEnum>
{
    public static readonly AllegroStatusEnum New = new("NEW", 0);
    public static readonly AllegroStatusEnum Processing = new("PROCESSING", 1);
    public static readonly AllegroStatusEnum ReadyForShipment = new("READY_FOR_SHIPMENT", 2);
    public static readonly AllegroStatusEnum ReadyForPickup = new("READY_FOR_PICKUP", 3);
    public static readonly AllegroStatusEnum Sent = new("SENT", 4);
    public static readonly AllegroStatusEnum PickedUp = new("PICKED_UP", 5);
    public static readonly AllegroStatusEnum Cancelled = new("CANCELLED", 6);
    public static readonly AllegroStatusEnum Suspended = new("SUSPENDED", 7);
    
    private AllegroStatusEnum(string name, int value) : base(name, value)
    {
    }
}
