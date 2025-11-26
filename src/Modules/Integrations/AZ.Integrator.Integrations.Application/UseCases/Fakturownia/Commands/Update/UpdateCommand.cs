using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Update;

public record UpdateCommand(UpdateFakturowniaIntegrationRequest Request) : ICommand<FakturowniaIntegrationViewModel>;



