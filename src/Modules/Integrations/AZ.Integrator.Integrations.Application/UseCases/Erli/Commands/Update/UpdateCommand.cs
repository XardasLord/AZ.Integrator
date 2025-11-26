using AZ.Integrator.Integrations.Contracts.RequestDtos;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using Mediator;

namespace AZ.Integrator.Integrations.Application.UseCases.Erli.Commands.Update;

public record UpdateCommand(UpdateErliIntegrationRequest Request) : ICommand<ErliIntegrationViewModel>;



