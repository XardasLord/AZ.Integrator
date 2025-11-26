using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Shopify;

public class ShopifyIntegration : Entity, IAggregateRoot
{
    private TenantId _tenantId;
    private SourceSystemId _sourceSystemId;
    private string _apiUrl;
    private string _adminApiToken;
    private string _displayName;
    private bool _isEnabled;
    private DateTimeOffset _createdAt;
    private DateTimeOffset _updatedAt;
    private SoftDeleteInfo _softDeleteInfo;

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string ApiUrl => _apiUrl;
    public string AdminApiToken => _adminApiToken;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    public SoftDeleteInfo SoftDeleteInfo => _softDeleteInfo;
    
    private ShopifyIntegration()
    {
    }
    
    public static ShopifyIntegration Create(
        SourceSystemId sourceSystemId,
        string apiUrl,
        string adminApiToken,
        string displayName,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        var integration = new ShopifyIntegration
        {
            _tenantId = currentUser.TenantId,
            _sourceSystemId = sourceSystemId,
            _apiUrl = apiUrl,
            _adminApiToken = adminApiToken,
            _displayName = displayName,
            _isEnabled = true,
            _createdAt = DateTimeOffset.UtcNow,
            _updatedAt = currentDateTime.CurrentDate(),
            _softDeleteInfo = SoftDeleteInfo.NotDeleted()
        };

        return integration;
    }

    public void Update(
        string apiUrl,
        string adminApiToken,
        string displayName,
        ICurrentDateTime currentDateTime)
    {
        _apiUrl = apiUrl;
        _adminApiToken = adminApiToken;
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