using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Queries.GetDpdLabel;

public record GetDpdLabelQuery(long SessionNumber) : IQuery<GetDocumentResponse>;