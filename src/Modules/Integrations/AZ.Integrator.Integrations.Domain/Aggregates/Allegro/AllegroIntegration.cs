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

    public TenantId TenantId => _tenantId;
    public SourceSystemId SourceSystemId => _sourceSystemId;
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string ClientId => _clientId;
    public string ClientSecret => _clientSecret;
    public string RedirectUri => _redirectUri;
    public string DisplayName => _displayName;
    public bool IsEnabled => _isEnabled;
    public DateTimeOffset CreatedAt => _createdAt;
    public DateTimeOffset UpdatedAt => _updatedAt;
    
    private AllegroIntegration()
    {
    }
}