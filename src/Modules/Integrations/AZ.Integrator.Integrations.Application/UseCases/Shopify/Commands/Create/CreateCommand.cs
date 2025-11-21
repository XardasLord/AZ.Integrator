using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Create;

public record CreateCommand(AddShopifyIntegrationRequest Request) : ICommand<ShopifyIntegrationViewModel>;