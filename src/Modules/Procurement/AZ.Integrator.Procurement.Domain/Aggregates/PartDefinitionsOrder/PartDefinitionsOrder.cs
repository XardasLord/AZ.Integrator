using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using AZ.Integrator.Procurement.Domain.Events;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;

public class PartDefinitionsOrder : Entity<OrderId>, IAggregateRoot
{
    private OrderNumber _number = null!;
    private TenantId _tenantId = null!;
    private SupplierId _supplierId = null!;
    private OrderStatus _status;
    private TenantCreationInformation _creationInformation = null!;
    private ModificationInformation _modificationInformation = null!;

    private List<FurnitureModelLine> _furnitureModelLines;

    public OrderNumber Number => _number;
    public TenantId TenantId => _tenantId;
    public SupplierId SupplierId => _supplierId;
    public OrderStatus Status => _status;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public ModificationInformation ModificationInformation => _modificationInformation;
    
    public IReadOnlyCollection<FurnitureModelLine> FurnitureModelLines => _furnitureModelLines.AsReadOnly();

    private PartDefinitionsOrder()
    {
        _furnitureModelLines = [];
        _creationInformation = null!;
        _modificationInformation = null!;
    }

    public static PartDefinitionsOrder Create(
        SupplierId supplierId,
        IEnumerable<FurnitureModelLineData> furnitureModelLinesData,
        IOrderNumberGenerator orderNumberGenerator,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        var date = currentDateTime.CurrentDate();
        
        var order = new PartDefinitionsOrder
        {
            _number = orderNumberGenerator.Generate(),
            _tenantId = currentUser.TenantId,
            _supplierId = supplierId ?? throw new ArgumentNullException(nameof(supplierId)),
            _status = OrderStatus.Registered,
            _creationInformation = new TenantCreationInformation(
                date,
                currentUser.UserId,
                currentUser.TenantId),
            _modificationInformation = new ModificationInformation(date, currentUser.UserId)
        };

        foreach (var lineData in furnitureModelLinesData)
            order.AddFurnitureModelLine(lineData);
        
        order.AddDomainEvent(new PartDefinitionsOrderRegistered(
            order.Number.Value,
            order.SupplierId.Value,
            order.TenantId.Value));

        return order;
    }

    // TODO: To verify if Update is needed
    public void UpdateFurnitureModelLines(
        IEnumerable<FurnitureModelLineData> furnitureModelLinesData,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        if (_status != OrderStatus.Registered)
            throw new InvalidOperationException($"Cannot update order in status {_status}");

        var linesData = furnitureModelLinesData.ToList();
        
        DeleteRemovedFurnitureModelLines(linesData);
        
        linesData.ForEach(lineData =>
        {
            if (lineData.FurnitureModelLineId.HasValue)
                UpdateFurnitureModelLine(lineData);
            else
                AddFurnitureModelLine(lineData);
        });

        _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public void MarkAsSent(ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        if (_status != OrderStatus.Registered)
            throw new InvalidOperationException($"Cannot mark order as sent from status {_status}");

        _status = OrderStatus.Sent;
        _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        
        AddDomainEvent(new PartDefinitionsOrderSent(
            Id.Value,
            Number.Value,
            SupplierId.Value,
            TenantId.Value));
    }

    private void AddFurnitureModelLine(FurnitureModelLineData lineData)
    {
        var line = FurnitureModelLine.Create(
            lineData.FurnitureCode,
            lineData.FurnitureVersion,
            lineData.QuantityOrdered,
            lineData.PartDefinitionLines,
            TenantId);

        _furnitureModelLines.Add(line);
    }

    private void UpdateFurnitureModelLine(FurnitureModelLineData lineData)
    {
        if (!lineData.FurnitureModelLineId.HasValue)
            throw new ArgumentException("FurnitureModelLineId must have value for update operation");

        var line = _furnitureModelLines.FirstOrDefault(l => l.Id == lineData.FurnitureModelLineId.Value)
            ?? throw new ArgumentException($"Furniture model line with ID {lineData.FurnitureModelLineId.Value} not found");

        line.Update(lineData.QuantityOrdered, lineData.PartDefinitionLines);
    }

    private void DeleteRemovedFurnitureModelLines(IEnumerable<FurnitureModelLineData> furnitureModelLinesData)
    {
        var existingLineIds = _furnitureModelLines.Select(l => l.Id).ToHashSet();
        
        var dataLineIds = furnitureModelLinesData
            .Where(ld => ld.FurnitureModelLineId.HasValue)
            .Select(ld => ld.FurnitureModelLineId!.Value)
            .ToHashSet();
        
        var lineIdsToDelete = existingLineIds.Except(dataLineIds).ToList();
        
        lineIdsToDelete.ForEach(RemoveFurnitureModelLine);
    }

    private void RemoveFurnitureModelLine(uint lineId)
    {
        var line = _furnitureModelLines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new ArgumentException($"Furniture model line with ID {lineId} not found");

        _furnitureModelLines.Remove(line);
    }
}

/// <summary>
/// Data transfer object for creating or updating furniture model lines in a part definitions order.
/// </summary>
public sealed record FurnitureModelLineData(
    uint? FurnitureModelLineId,
    string FurnitureCode,
    int FurnitureVersion,
    Quantity QuantityOrdered,
    IEnumerable<PartDefinitionLineData> PartDefinitionLines);

/// <summary>
/// Data transfer object for creating or updating part definition lines.
/// Represents a snapshot of furniture part specifications at the time of order creation.
/// </summary>
public sealed record PartDefinitionLineData(
    uint? PartDefinitionLineId,
    PartName PartName,
    Dimensions Dimensions,
    Quantity Quantity,
    string? AdditionalInfo);
