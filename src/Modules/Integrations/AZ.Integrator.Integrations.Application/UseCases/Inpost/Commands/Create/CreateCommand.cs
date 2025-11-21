using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Inpost.Commands.Create;

public record CreateCommand(AddInpostIntegrationRequest Request) : ICommand<InpostIntegrationViewModel>;