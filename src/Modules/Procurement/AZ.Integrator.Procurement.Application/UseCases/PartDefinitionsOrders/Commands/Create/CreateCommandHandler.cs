using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.Commands.Create;

public class CreateCommandHandler(
    IAggregateRepository<PartDefinitionsOrder> repository,
    IOrderNumberGenerator orderNumberGenerator,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : ICommandHandler<CreateCommand, PartDefinitionsOrderViewModel>
{
    public async ValueTask<PartDefinitionsOrderViewModel> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        var furnitureLines = command.FurnitureLineRequests
            .Select(x => new FurnitureModelLineData(
                null,
                x.FurnitureCode,
                x.FurnitureVersion,
                x.QuantityOrdered,
                x.PartDefinitionLines.Select(pd =>
                    new PartDefinitionLineData(
                        null,
                        pd.PartName,
                        new Dimensions(
                            pd.LengthMm,
                            pd.WidthMm,
                            pd.ThicknessMm,
                            (EdgeBandingType)pd.LengthEdgeBandingType,
                            (EdgeBandingType)pd.WidthEdgeBandingType),
                        pd.Quantity,
                        pd.AdditionalInfo))));
        
        var order = PartDefinitionsOrder.Create(
            command.SupplierId,
            furnitureLines,
            command.AdditionalNotes,
            orderNumberGenerator,
            currentUser, 
            currentDateTime);

        await repository.AddAsync(order, cancellationToken);

        return new PartDefinitionsOrderViewModel
        {
            Id = (int)order.Id.Value,
            Number = order.Number,
            SupplierId = (int)order.SupplierId.Value,
            AdditionalNotes = order.AdditionalNotes,
            Status = (OrderStatusViewModel)order.Status,
            CreatedBy = order.CreationInformation.CreatedBy,
            CreatedAt = order.CreationInformation.CreatedAt.Date,
            ModifiedBy = order.ModificationInformation.ModifiedBy,
            ModifiedAt = order.ModificationInformation.ModifiedAt.Date,
            FurnitureLines = order.FurnitureModelLines.Select(fm => new OrderFurnitureLineViewModel
            {
                Id = (int)fm.Id.Value,
                OrderId = (int)order.Id.Value,
                TenantId = order.TenantId,
                FurnitureCode = fm.FurnitureCode,
                FurnitureVersion = fm.FurnitureVersion,
                QuantityOrdered = fm.QuantityOrdered.Value,
                PartLines = fm.Lines.Select(pl => new OrderFurniturePartLineViewModel
                {
                    Id = (int)pl.Id.Value,
                    OrderFurnitureLineId = (int)fm.Id.Value,
                    Name = pl.PartName,
                    Dimensions = new OrderFurniturePartLineDimensionsViewModel
                    {
                        LengthMm = pl.Dimensions.LengthMm,
                        WidthMm = pl.Dimensions.WidthMm,
                        ThicknessMm = pl.Dimensions.ThicknessMm,
                        LengthEdgeBandingType = (OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel)pl.Dimensions.LengthEdgeBandingType,
                        WidthEdgeBandingType = (OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel)pl.Dimensions.WidthEdgeBandingType
                    },
                    Quantity = pl.Quantity.Value,
                    AdditionalInfo = pl.AdditionalInfo
                }).ToList()
            }).ToList()
        };
    }
}