using AutoMapper;
using AZ.Integrator.Orders.Contracts.Dtos;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Orders.Application.Common.AutoMapper;

public class OrderDetailsDtoMapper : Profile
{
    public OrderDetailsDtoMapper()
    {
        CreateMap<GetOrderDetailsModelResponse, OrderDetailsDto>();
        CreateMap<BuyerDetails, BuyerDetailsDto>();
        CreateMap<PreferencesDetails, PreferencesDetailsDto>();
        CreateMap<AddressDetails, AddressDetailsDto>();
        CreateMap<PaymentDetails, PaymentDetailsDto>();
        CreateMap<AmountDetails, AmountDetailsDto>();
        CreateMap<FulfillmentDetails, FulfillmentDetailsDto>();
        CreateMap<ShipmentSummaryDetails, ShipmentSummaryDetailsDto>();
        CreateMap<DeliveryDetails, DeliveryDetailsDto>();
        CreateMap<DeliveryAddressDetails, DeliveryAddressDetailsDto>();
        CreateMap<MethodDetails, MethodDetailsDto>();
        CreateMap<PickupPointDetails, PickupPointDetailsDto>();
        CreateMap<TimeDetails, TimeDetailsDto>();
        CreateMap<GuaranteedDetails, GuaranteedDetailsDto>();
        CreateMap<DispatchDetails, DispatchDetailsDto>();
        CreateMap<InvoiceDetails, InvoiceDetailsDto>();
        CreateMap<LineItemDetails, LineItemDetailsDto>();
        CreateMap<OfferDetails, OfferDetailsDto>();
        CreateMap<ExternalDetails, ExternalDetailsDto>();
        // CreateMap<ReconciliationDetails, ReconciliationDetailsDto>();
        // CreateMap<AdditionalServiceDetails, AdditionalServiceDetailsDto>();
        CreateMap<SurchargeDetails, SurchargeDetailsDto>();
        CreateMap<DiscountDetails, DiscountDetailsDto>();
        CreateMap<NoteDetails, NoteDetailsDto>();
        // CreateMap<MarketplaceDetails, MarketplaceDetailsDto>();
        CreateMap<SummaryDetails, SummaryDetailsDto>();
    }
}