using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Shared.Application;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace AZ.Integrator.Shared.Infrastructure.Mediator;

public class HeaderRequestMiddleware<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor,
    ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IMessage
{
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;

        if (request is not HeaderRequest headerRequest || context is null)
            return await next(request, cancellationToken);
        
        // TODO: Can be removed after introduction of SourceSystemId
        var shopProviderHeader = context.Request.Headers["Az-Integrator-Shop-Provider"].ToString();
        if (Enum.TryParse<ShopProviderType>(shopProviderHeader, true, out var shopProvider))
        {
            headerRequest.ShopProvider = shopProvider;
        }
            
        headerRequest.SourceSystemId = context.Request.Headers["Az-Integrator-Source-System-Id"].ToString();
        headerRequest.TenantId = currentUser.TenantId;

        return await next(request, cancellationToken);
    }
}