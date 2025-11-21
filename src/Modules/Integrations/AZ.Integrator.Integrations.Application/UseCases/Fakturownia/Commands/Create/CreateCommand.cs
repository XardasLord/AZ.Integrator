using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Fakturownia.Commands.Create;

public record CreateCommand(AddFakturowniaIntegrationRequest Request) : ICommand<FakturowniaIntegrationViewModel>;