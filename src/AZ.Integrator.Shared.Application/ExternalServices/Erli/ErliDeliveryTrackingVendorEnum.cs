using Ardalis.SmartEnum;

namespace AZ.Integrator.Shared.Application.ExternalServices.Erli;

public sealed class ErliDeliveryTrackingVendorEnum : SmartEnum<ErliDeliveryTrackingVendorEnum>
{
    public static readonly ErliDeliveryTrackingVendorEnum InPost = new("inpost", 0);
    public static readonly ErliDeliveryTrackingVendorEnum PocztaPolska = new("pocztaPolska", 1);
    public static readonly ErliDeliveryTrackingVendorEnum Dhl = new("dhl", 2);
    public static readonly ErliDeliveryTrackingVendorEnum Dpd = new("dpd", 3);
    public static readonly ErliDeliveryTrackingVendorEnum Dts = new("dts", 4);
    public static readonly ErliDeliveryTrackingVendorEnum Fedex = new("fedex", 5);
    public static readonly ErliDeliveryTrackingVendorEnum Pocztex24 = new("pocztex24", 6);
    // Also many others available from this documentation -> https://erli.pl/svc/shop-api/doc/reference/#/order/patch_orders__id_
    
    private ErliDeliveryTrackingVendorEnum(string name, int value) : base(name, value)
    {
    }
}
