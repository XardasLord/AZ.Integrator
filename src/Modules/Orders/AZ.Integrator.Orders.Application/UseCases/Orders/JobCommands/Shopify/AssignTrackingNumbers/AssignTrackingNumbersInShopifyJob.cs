using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignTrackingNumbers;

public class AssignTrackingNumbersInShopifyJob(IMediator mediator) : JobBase<AssignTrackingNumbersInShopifyJobCommand>(mediator);