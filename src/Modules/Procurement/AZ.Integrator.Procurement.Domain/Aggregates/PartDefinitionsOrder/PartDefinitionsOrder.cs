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
    private string? _additionalNotes;
    private OrderStatus _status;
    private TenantCreationInformation _creationInformation = null!;
    private ModificationInformation _modificationInformation = null!;

    private List<OrderFurnitureLine> _furnitureModelLines;

    public OrderNumber Number => _number;
    public TenantId TenantId => _tenantId;
    public SupplierId SupplierId => _supplierId;
    public string? AdditionalNotes => _additionalNotes;
    public OrderStatus Status => _status;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public ModificationInformation ModificationInformation => _modificationInformation;
    
    public IReadOnlyCollection<OrderFurnitureLine> FurnitureModelLines => _furnitureModelLines.AsReadOnly();

    private PartDefinitionsOrder()
    {
        _furnitureModelLines = [];
        _creationInformation = null!;
        _modificationInformation = null!;
    }

    public static PartDefinitionsOrder Create(
        SupplierId supplierId,
        IEnumerable<FurnitureModelLineData> furnitureModelLinesData,
        string additionalNotes,
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
            _additionalNotes = additionalNotes,
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
        var line = OrderFurnitureLine.Create(
            lineData.FurnitureCode,
            lineData.FurnitureVersion,
            lineData.QuantityOrdered,
            lineData.PartDefinitionLines,
            TenantId);

        _furnitureModelLines.Add(line);
    }
}

public sealed record FurnitureModelLineData(
    string FurnitureCode,
    int FurnitureVersion,
    Quantity QuantityOrdered,
    IEnumerable<PartDefinitionLineData> PartDefinitionLines);

public sealed record PartDefinitionLineData(
    uint? PartDefinitionLineId,
    PartName PartName,
    Dimensions Dimensions,
    Quantity Quantity,
    string? AdditionalInfo);
