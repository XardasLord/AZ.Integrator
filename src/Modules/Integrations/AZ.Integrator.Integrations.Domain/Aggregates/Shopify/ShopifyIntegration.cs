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

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string ApiUrl => _apiUrl;
    public string AdminApiToken => _adminApiToken;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    
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
            _updatedAt = currentDateTime.CurrentDate()
        };

        return integration;
    }
}