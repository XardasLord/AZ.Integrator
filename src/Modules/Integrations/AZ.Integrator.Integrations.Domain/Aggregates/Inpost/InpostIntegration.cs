using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Inpost;

public class InpostIntegration : Entity, IAggregateRoot
{
    private TenantId _tenantId;
    private int _organizationId;
    private string _accessToken;
    private string _displayName;
    private bool _isEnabled;
    private DateTimeOffset _createdAt;
    private DateTimeOffset _updatedAt;
    private SenderData _senderData;

    public TenantId TenantId => _tenantId;
    public int OrganizationId => _organizationId;
    public string AccessToken => _accessToken;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    public SenderData SenderData => _senderData;
    
    private InpostIntegration()
    {
    }
    
    public static InpostIntegration Create(
        int organizationId,
        string accessToken,
        string displayName,
        SenderData senderData,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        var integration = new InpostIntegration
        {
            _tenantId = currentUser.TenantId,
            _organizationId = organizationId,
            _accessToken = accessToken,
            _displayName = displayName,
            _isEnabled = true,
            _createdAt = DateTimeOffset.UtcNow,
            _updatedAt = currentDateTime.CurrentDate(),
            _senderData = senderData
        };

        return integration;
    }
}