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
    private SoftDeleteInfo _softDeleteInfo;

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string ApiKey => _apiKey;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    public SoftDeleteInfo SoftDeleteInfo => _softDeleteInfo;
    
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
            _updatedAt = currentDateTime.CurrentDate(),
            _softDeleteInfo = SoftDeleteInfo.NotDeleted()
        };

        return integration;
    }

    public void Update(
        string apiKey,
        string displayName,
        ICurrentDateTime currentDateTime)
    {
        _apiKey = apiKey;
        _displayName = displayName;
        _updatedAt = currentDateTime.CurrentDate();
    }

    public void SetEnabled(bool isEnabled, ICurrentDateTime currentDateTime)
    {
        _isEnabled = isEnabled;
        _updatedAt = currentDateTime.CurrentDate();
    }

    public void Delete(ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _softDeleteInfo = SoftDeleteInfo.Deleted(currentDateTime.CurrentDate(), currentUser.UserId);
        SetEnabled(false, currentDateTime);
    }
}