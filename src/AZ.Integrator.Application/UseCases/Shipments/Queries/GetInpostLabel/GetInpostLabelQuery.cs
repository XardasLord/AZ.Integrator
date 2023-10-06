using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.Queries.GetInpostLabel;

public record GetInpostLabelQuery(string ShipmentNumber) : IQuery<GetDocumentResponse>;