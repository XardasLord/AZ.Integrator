using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;

public class AssignInvoiceInShopifyJob(IMediator mediator) : JobBase<AssignInvoiceInShopifyJobCommand>(mediator);