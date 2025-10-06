using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Shared.Application;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.Mediator;

public class HeaderRequestMiddleware<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IMessage
{
    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var context = httpContextAccessor.HttpContext;

        if (request is not HeaderRequest headerRequest || context is null)
            return await next(request, cancellationToken);
        
        var shopProviderHeader = context.Request.Headers["Az-Integrator-Shop-Provider"].ToString();
        if (Enum.TryParse<ShopProviderType>(shopProviderHeader, true, out var shopProvider))
        {
            headerRequest.ShopProvider = shopProvider;
        }
            
        // TODO: TenantId should be obtained from the authentication token
        headerRequest.TenantId = context.Request.Headers["Az-Integrator-Tenant-Id"].ToString();
        headerRequest.SourceSystemId = context.Request.Headers["Az-Integrator-Source-System-Id"].ToString();

        return await next(request, cancellationToken);
    }
}