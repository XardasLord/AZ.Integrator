using Ardalis.SmartEnum;

namespace AZ.Integrator.Monitoring.Contracts;

public sealed class MonitoringSourceModuleEnum : SmartEnum<MonitoringSourceModuleEnum>
{
    public static readonly MonitoringSourceModuleEnum Orders = new("Orders", 0);
    public static readonly MonitoringSourceModuleEnum Shipments = new("Shipments", 1);
    public static readonly MonitoringSourceModuleEnum Invoices = new("Invoices", 2);
    
    private MonitoringSourceModuleEnum(string name, int value) : base(name, value)
    {
    }
}
