using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Create;

public record CreateCommand(AddErliIntegrationRequest Request) : ICommand<ErliIntegrationViewModel>;