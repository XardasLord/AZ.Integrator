using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Shopify.Commands.Update;

public record UpdateCommand(UpdateShopifyIntegrationRequest Request) : ICommand<ShopifyIntegrationViewModel>;

