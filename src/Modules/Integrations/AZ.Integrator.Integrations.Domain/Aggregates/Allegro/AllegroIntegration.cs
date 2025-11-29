using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Integrations.Domain.Aggregates.Allegro;

public class AllegroIntegration : Entity, IAggregateRoot
{
    private TenantId _tenantId;
    private SourceSystemId _sourceSystemId;
    private string _accessToken;
    private string _refreshToken;
    private DateTime _expiresAt;
    private string _clientId;
    private string _clientSecret;
    private string _redirectUri;
    private string _displayName;
    private bool _isEnabled;
    private DateTimeOffset _createdAt;
    private DateTimeOffset _updatedAt;
    private SoftDeleteInfo _softDeleteInfo;

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string AccessToken => _accessToken;
    public string RefreshToken => _refreshToken;
    public DateTime ExpiresAt => _expiresAt;
    
    [Obsolete("Will be removed.")]
    public string ClientId => _clientId;
    [Obsolete("Will be removed.")]
    public string ClientSecret => _clientSecret;
    [Obsolete("Will be removed.")]
    public string RedirectUri => _redirectUri;
    
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    public SoftDeleteInfo SoftDeleteInfo => _softDeleteInfo;
    
    private AllegroIntegration()
    {
    }
    
    public static AllegroIntegration Create(
        SourceSystemId sourceSystemId,
        Guid tenantId,
        string accessToken,
        string refreshToken,
        DateTime expiresAt,
        string displayName,
        ICurrentUser currentUser,
        ICurrentDateTime currentDateTime)
    {
        var integration = new AllegroIntegration
        {
            _tenantId = tenantId,
            _sourceSystemId = sourceSystemId,
            _accessToken = accessToken,
            _refreshToken = refreshToken,
            _expiresAt = expiresAt,
            _clientId = "",
            _clientSecret = "",
            _redirectUri = "",
            _displayName = displayName,
            _isEnabled = true,
            _createdAt = DateTimeOffset.UtcNow,
            _updatedAt = currentDateTime.CurrentDate(),
            _softDeleteInfo = SoftDeleteInfo.NotDeleted()
        };

        return integration;
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
    
    public void UpdateTokens(string newAccessToken, string newRefreshToken, DateTime newExpiresAt, ICurrentDateTime currentDateTime)
    {
        _accessToken = newAccessToken;
        _refreshToken = newRefreshToken;
        _expiresAt = newExpiresAt;
        _updatedAt = currentDateTime.CurrentDate();
    }
}