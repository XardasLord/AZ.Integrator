using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;

public class OrderFurnitureLine : Entity<OrderFurnitureLineId>
{
    private string _furnitureCode;
    private int _furnitureVersion;
    private Quantity _quantityOrdered = null!;
    private TenantId _tenantId = null!;
    
    private List<OrderFurniturePartLine> _lines;
    
    public string FurnitureCode => _furnitureCode;
    public int FurnitureVersion => _furnitureVersion;
    public Quantity QuantityOrdered => _quantityOrdered;
    public TenantId TenantId => _tenantId;
    
    public IReadOnlyCollection<OrderFurniturePartLine> Lines => _lines.AsReadOnly();

    private OrderFurnitureLine()
    {
        _lines = [];
    }

    internal static OrderFurnitureLine Create(
        string furnitureCode,
        int furnitureVersion,
        Quantity quantityOrdered,
        IEnumerable<PartDefinitionLineData> partDefinitionLinesData,
        TenantId tenantId)
    {
        if (quantityOrdered.Value <= 0)
            throw new ArgumentException("Quantity ordered must be greater than 0", nameof(quantityOrdered));

        var line = new OrderFurnitureLine
        {
            _furnitureCode = furnitureCode,
            _furnitureVersion = furnitureVersion,
            _quantityOrdered = quantityOrdered,
            _tenantId = tenantId
        };

        foreach (var lineData in partDefinitionLinesData)
            line.AddPartDefinitionLine(lineData);

        if (line._lines.Count == 0)
            throw new InvalidOperationException("Furniture model line must have at least one part definition line");

        return line;
    }

    internal void Update(Quantity quantityOrdered, IEnumerable<PartDefinitionLineData> partDefinitionLinesData)
    {
        if (quantityOrdered.Value <= 0)
            throw new ArgumentException("Quantity ordered must be greater than 0", nameof(quantityOrdered));

        _quantityOrdered = quantityOrdered;
        
        var linesData = partDefinitionLinesData.ToList();
        
        DeleteRemovedPartDefinitionLines(linesData);
        
        linesData.ForEach(lineData =>
        {
            if (lineData.PartDefinitionLineId.HasValue)
                UpdatePartDefinitionLine(lineData);
            else
                AddPartDefinitionLine(lineData);
        });

        if (_lines.Count == 0)
            throw new InvalidOperationException("Furniture model line must have at least one part definition line");
    }

    private void AddPartDefinitionLine(PartDefinitionLineData lineData)
    {
        var line = OrderFurniturePartLine.Create(
            lineData.PartName,
            lineData.Dimensions,
            lineData.Quantity,
            lineData.AdditionalInfo);

        _lines.Add(line);
    }

    private void UpdatePartDefinitionLine(PartDefinitionLineData lineData)
    {
        if (!lineData.PartDefinitionLineId.HasValue)
            throw new ArgumentException("PartDefinitionLineId must have value for update operation");

        var line = _lines.FirstOrDefault(l => l.Id == lineData.PartDefinitionLineId.Value)
                   ?? throw new ArgumentException($"Part definition line with ID {lineData.PartDefinitionLineId.Value} not found");

        line.Update(lineData.PartName, lineData.Dimensions, lineData.Quantity, lineData.AdditionalInfo);
    }

    private void DeleteRemovedPartDefinitionLines(IEnumerable<PartDefinitionLineData> partDefinitionLinesData)
    {
        var existingLineIds = _lines.Select(l => l.Id).ToHashSet();
        
        var dataLineIds = partDefinitionLinesData
            .Where(ld => ld.PartDefinitionLineId.HasValue)
            .Select(ld => (OrderFurniturePartLineId)ld.PartDefinitionLineId!.Value)
            .ToHashSet();
        
        var lineIdsToDelete = existingLineIds.Except(dataLineIds).ToList();
        
        lineIdsToDelete.ForEach(RemovePartDefinitionLine);
    }

    private void RemovePartDefinitionLine(OrderFurniturePartLineId lineId)
    {
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
                   ?? throw new ArgumentException($"Part definition line with ID {lineId} not found");

        _lines.Remove(line);
    }
}