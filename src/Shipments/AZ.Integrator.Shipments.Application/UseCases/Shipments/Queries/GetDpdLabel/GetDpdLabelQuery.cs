
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetDpdLabel;

public record GetDpdLabelQuery(long SessionNumber) : IRequest<GetDocumentResponse>;