using System.Globalization;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;

public class RegisterInvoiceCommandHandler : IRequestHandler<RegisterInvoiceCommand>
{
    private readonly IInvoiceService _invoiceService;
    private readonly IAllegroService _allegroService;
    private readonly IErliService _erliService;
    private readonly IShopifyService _shopifyService;
    private readonly IAggregateRepository<Invoice> _invoiceRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentDateTime _currentDateTime;

    public RegisterInvoiceCommandHandler(
        IInvoiceService invoiceService,
        IAllegroService allegroService,
        IErliService erliService,
        IShopifyService shopifyService,
        IAggregateRepository<Invoice> invoiceRepository,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        _invoiceService = invoiceService;
        _allegroService = allegroService;
        _erliService = erliService;
        _shopifyService = shopifyService;
        _invoiceRepository = invoiceRepository;
        _currentUser = currentUser;
        _currentDateTime = currentDateTime;
    }
    
    public async ValueTask<Unit> Handle(RegisterInvoiceCommand command, CancellationToken cancellationToken)
    {
        var invoiceResponse = command.ShopProvider switch
        {
            ShopProviderType.Allegro => await GenerateInvoiceFromAllegro(command),
            ShopProviderType.Erli => await GenerateInvoiceFromErli(command),
            ShopProviderType.Shopify => await GenerateInvoiceFromShopify(command),
            ShopProviderType.System or ShopProviderType.Unknown => throw new NotImplementedException(
                "Unknown shop provider"),
            _ => throw new ArgumentOutOfRangeException()
        };

        var invoice = Invoice.Create(invoiceResponse?.Id, invoiceResponse?.Number, command.OrderNumber, command.TenantId, _currentUser, _currentDateTime);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        return Unit.Value;
    }

    private async ValueTask<CreateInvoiceResponse> GenerateInvoiceFromAllegro(RegisterInvoiceCommand command)
    {
        var orderDetails = await _allegroService.GetOrderDetails(Guid.Parse(command.OrderNumber), command.TenantId);
                
        var buyerDetails = new BuyerDetails(
            orderDetails.Buyer.Email,
            orderDetails.Buyer.FirstName,
            orderDetails.Buyer.LastName,
            orderDetails.Buyer.CompanyName?.ToString(),
            orderDetails.Buyer.PersonalIdentity,
            orderDetails.Buyer.PhoneNumber?.ToString());

        var invoiceItems = orderDetails.LineItems.Select(x =>
                new InvoiceItem(x.Offer.Name,
                    decimal.Parse(x.Price.Amount, CultureInfo.InvariantCulture),
                    x.Quantity,
                    x.Price.Currency))
            .ToList();

        var paymentDetails = new PaymentDetails(
            orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            orderDetails.Payment.FinishedAt ?? DateTime.UtcNow);

        var deliveryDetails = new DeliveryDetails(
            "KURIER",
            decimal.Parse(orderDetails.Delivery.Cost?.Amount ?? 0.ToString(),
                CultureInfo.InvariantCulture));
                
        var response = await _invoiceService.GenerateInvoice(buyerDetails, invoiceItems, paymentDetails, deliveryDetails);

        return response;
    }

    private async ValueTask<CreateInvoiceResponse> GenerateInvoiceFromErli(RegisterInvoiceCommand command)
    {
        var orderDetails = await _erliService.GetOrderDetails(command.OrderNumber, command.TenantId);
                
        var buyerDetails = new BuyerDetails(
            orderDetails.User?.Email,
            orderDetails.InvoiceAddress?.FirstName ?? orderDetails.User?.DeliveryAddress?.FirstName,
            orderDetails.InvoiceAddress?.LastName ?? orderDetails.User?.DeliveryAddress?.LastName,
            orderDetails.InvoiceAddress?.CompanyName,
            orderDetails.InvoiceAddress?.Nip,
            null);
        
        var invoiceItems = orderDetails.Items.Select(x =>
                new InvoiceItem(x.Name,
                    decimal.Parse((x.UnitPrice / 100).ToString(), CultureInfo.InvariantCulture),
                    x.Quantity,
                    "PL"))
            .ToList();
        
        var paymentDetails = new PaymentDetails(
            orderDetails.PurchasedAt.Date,
            orderDetails.PurchasedAt.Date,
            orderDetails.PurchasedAt.Date);
        
        var deliveryDetails = new DeliveryDetails(
            orderDetails.Delivery.Name,
            decimal.Parse(((orderDetails.Delivery?.Price ?? 0) / 100).ToString(), CultureInfo.InvariantCulture));
        
        var invoiceResponse = await _invoiceService.GenerateInvoice(buyerDetails, invoiceItems, paymentDetails, deliveryDetails);
        
        return invoiceResponse;
    }
    
    private async ValueTask<CreateInvoiceResponse> GenerateInvoiceFromShopify(RegisterInvoiceCommand command)
    {
        var orderDetails = await _shopifyService.GetOrderDetails(command.OrderNumber, command.TenantId);
                
        var buyerDetails = new BuyerDetails(
            orderDetails.Email,
            orderDetails.BillingAddress?.FirstName,
            orderDetails.BillingAddress?.LastName,
            orderDetails.BillingAddress?.Company,
            null,
            orderDetails.BillingAddress?.Phone);

        var invoiceItems = orderDetails.LineItems.Nodes.Select(x =>
                new InvoiceItem(x.Name,
                    decimal.Parse(x.OriginalUnitPriceSet.PresentmentMoney.Amount, CultureInfo.InvariantCulture),
                    x.Quantity,
                    x.OriginalUnitPriceSet.PresentmentMoney.CurrencyCode))
            .ToList();

        var paymentDetails = new PaymentDetails(
            orderDetails.CreatedAt.Date,
            orderDetails.CreatedAt.Date,
            orderDetails.CreatedAt.Date);

        var deliveryDetails = new DeliveryDetails(
            orderDetails.ShippingLine.Title,
            decimal.Parse(orderDetails.ShippingLine?.CurrentDiscountedPriceSet?.PresentmentMoney?.Amount ?? 0.ToString(),
                CultureInfo.InvariantCulture));

        var invoiceResponse = await _invoiceService.GenerateInvoice(buyerDetails, invoiceItems, paymentDetails, deliveryDetails);
        
        return invoiceResponse;
    }
}