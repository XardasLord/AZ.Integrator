using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.AllegroAccount;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshTenantAccessTokenCommandHandler : JobCommandHandlerBase<RefreshTenantAccessTokenCommand>
{
    private readonly AllegroAccountDbContext _allegroAccountDbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AllegroOptions _allegroOptions;

    public RefreshTenantAccessTokenCommandHandler(
        AllegroAccountDbContext allegroAccountDbContext,
        IHttpClientFactory httpClientFactory,
        AllegroOptions allegroOptions)
    {
        _allegroAccountDbContext = allegroAccountDbContext;
        _httpClientFactory = httpClientFactory;
        _allegroOptions = allegroOptions;
    }

    public override async ValueTask<Unit> Handle(RefreshTenantAccessTokenCommand command, CancellationToken cancellationToken)
    {
        await base.Handle(command, cancellationToken);
        
        var tenantAccount = await _allegroAccountDbContext.AllegroAccounts
            .SingleAsync(x => x.TenantId == command.TenantId, cancellationToken);

        var queryParams = PrepareQueryFilters(tenantAccount);
        var httpClient = PrepareHttpClient(tenantAccount.ClientId, tenantAccount.ClientSecret);
        
        using var response = await httpClient.PostAsync($"{_allegroOptions.TokenEndpoint}?{queryParams}", null, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var refreshAccessTokenResponse = await response.Content.ReadFromJsonAsync<RefreshAccessTokenResponse>(cancellationToken: cancellationToken);

        tenantAccount.AccessToken = refreshAccessTokenResponse.access_token;
        tenantAccount.RefreshToken = refreshAccessTokenResponse.refresh_token;
        tenantAccount.ExpiresAt = GetExpiryTimestamp(refreshAccessTokenResponse.access_token);

        await _allegroAccountDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
    
    private static string PrepareQueryFilters(AllegroAccountViewModel tenantAccount)
    {
        var queryParamsDictionary = new Dictionary<string, object>
        {
            { "grant_type", "refresh_token" }, 
            { "refresh_token", tenantAccount.RefreshToken }, 
            { "redirect_uri", tenantAccount.RedirectUri }
        };

        return queryParamsDictionary.ToHttpQueryString();
    }
    
    private HttpClient PrepareHttpClient(string clientId, string secretKey)
    {
        var httpClient = _httpClientFactory.CreateClient();
        
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secretKey}"));
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
        
        return httpClient;
    }
    
    private static DateTime GetExpiryTimestamp(string accessToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return DateTime.MinValue;
            if (!accessToken.Contains("."))
                return DateTime.MinValue;
 
            var parts = accessToken.Split('.');
            var payload = JsonSerializer.Deserialize<JwtToken>(Base64UrlEncoder.Decode(parts[1]));
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
            
            return dateTimeOffset.LocalDateTime;
        }
        catch (Exception)
        {
            return DateTime.UtcNow.AddHours(12);
        }
    }
}

public class JwtToken
{
    public long exp { get; set; }
}