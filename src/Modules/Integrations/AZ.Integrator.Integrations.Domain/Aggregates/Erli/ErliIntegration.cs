using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Erli;

public class ErliIntegration : Entity, IAggregateRoot
{
    private TenantId _tenantId;
    private SourceSystemId _sourceSystemId;
    private string _apiKey;
    private string _displayName;
    private bool _isEnabled;
    private DateTimeOffset _createdAt;
    private DateTimeOffset _updatedAt;

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string ApiKey => _apiKey;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    
    private ErliIntegration()
    {
    }
    
    public static ErliIntegration Create(
        SourceSystemId sourceSystemId,
        string apiKey,
        string displayName,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        var integration = new ErliIntegration
        {
            _tenantId = currentUser.TenantId,
            _sourceSystemId = sourceSystemId,
            _apiKey = apiKey,
            _displayName = displayName,
            _isEnabled = true,
            _createdAt = DateTimeOffset.UtcNow,
            _updatedAt = currentDateTime.CurrentDate()
        };

        return integration;
    }
}