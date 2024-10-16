using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetInpostLabels;

public record GetInpostLabelsQuery(IEnumerable<string> ShipmentNumber) : IRequest<GetDocumentResponse>;