using AZ.Integrator.Procurement.Domain.Events;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.EventHandlers;

/// <summary>
/// Event handler for PartDefinitionsOrderMarkedAsSent domain event.
/// Schedules a Hangfire background job to send order email to supplier.
/// </summary>
/// <remarks>
/// This is a PLACEHOLDER/EXAMPLE implementation.
/// TODO: Implement actual email sending logic via Hangfire.
/// </remarks>
public class SendOrderEmailToSupplierHandler : INotificationHandler<PartDefinitionsOrderRegistered>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public SendOrderEmailToSupplierHandler(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public ValueTask Handle(PartDefinitionsOrderRegistered notification, CancellationToken cancellationToken)
    {
        // Schedule a Hangfire background job to send the email
        // The job will:
        // 1. Load order details from repository
        // 2. Load supplier mailboxes
        // 3. Generate email content (possibly using a template)
        // 4. Send email via SMTP or email service
        
        _backgroundJobClient.Enqueue<ISendPartDefinitionsOrderEmailJob>(
            job => job.SendOrderEmailAsync(
                notification.SupplierId,
                notification.TenantId,
                CancellationToken.None));

        return ValueTask.CompletedTask;
    }
}

/// <summary>
/// Hangfire job interface for sending part definitions order emails.
/// </summary>
/// <remarks>
/// TODO: Implement this interface and register in DI container.
/// </remarks>
public interface ISendPartDefinitionsOrderEmailJob
{
    Task SendOrderEmailAsync(uint supplierId, Guid tenantId, CancellationToken cancellationToken);
}

/* IMPLEMENTATION EXAMPLE:

public class SendPartDefinitionsOrderEmailJob : ISendPartDefinitionsOrderEmailJob
{
    private readonly IAggregateReadRepository<PartDefinitionsOrder> _orderRepository;
    private readonly IAggregateReadRepository<Supplier> _supplierRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendPartDefinitionsOrderEmailJob> _logger;

    public SendPartDefinitionsOrderEmailJob(
        IAggregateReadRepository<PartDefinitionsOrder> orderRepository,
        IAggregateReadRepository<Supplier> supplierRepository,
        IEmailService emailService,
        ILogger<SendPartDefinitionsOrderEmailJob> logger)
    {
        _orderRepository = orderRepository;
        _supplierRepository = supplierRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendOrderEmailAsync(uint orderId, uint supplierId, Guid tenantId, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Load order
            var orderSpec = new PartDefinitionsOrderByIdSpec(orderId, tenantId);
            var order = await _orderRepository.FirstOrDefaultAsync(orderSpec, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found for tenant {TenantId}", orderId, tenantId);
                return;
            }

            // 2. Load supplier
            var supplierSpec = new SupplierByIdSpec(supplierId, tenantId);
            var supplier = await _supplierRepository.FirstOrDefaultAsync(supplierSpec, cancellationToken);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier {SupplierId} not found for tenant {TenantId}", supplierId, tenantId);
                return;
            }

            // 3. Generate email content
            var emailContent = GenerateEmailContent(order, supplier);
            
            // 4. Send email to all supplier mailboxes
            var mailboxes = supplier.Mailboxes.Select(m => m.Email.Value).ToList();
            await _emailService.SendEmailAsync(
                to: mailboxes,
                subject: $"Zamówienie formatek - {order.Number}",
                body: emailContent,
                isHtml: true,
                cancellationToken: cancellationToken);

            _logger.LogInformation("Order email sent successfully for order {OrderNumber}", order.Number);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send order email for order {OrderId}", orderId);
            throw; // Hangfire will retry
        }
    }

    private string GenerateEmailContent(PartDefinitionsOrder order, Supplier supplier)
    {
        // TODO: Use a templating engine (e.g., Razor, Scriban) to generate HTML email
        // Include:
        // - Order number
        // - Order date
        // - List of furniture models with quantities
        // - Part definitions with dimensions, quantities, edge banding
        // - Additional information
        // - Contact information
        
        return $"<html>...</html>";
    }
}

*/

