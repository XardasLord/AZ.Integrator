using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetInpostLabel;

public record GetInpostLabelQuery(string ShipmentNumber) : IRequest<GetDocumentResponse>;