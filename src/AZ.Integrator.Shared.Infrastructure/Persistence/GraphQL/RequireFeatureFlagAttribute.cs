using System.Reflection;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using HotChocolate.Types.Descriptors;
using Microsoft.Extensions.DependencyInjection;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;

public class RequireFeatureFlagAttribute(string featureFlagCode) : ObjectFieldDescriptorAttribute
{
    public string FeatureFlagCode { get; } = featureFlagCode;

    protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use(next => async context =>
        {
            var featureFlagService = context.Services.GetRequiredService<IFeatureFlags>();
            var currentUser = context.Services.GetRequiredService<ICurrentUser>();
            
            if (!await featureFlagService.IsEnabledAsync(FeatureFlagCode, currentUser.TenantId))
                throw new GraphQLException($"Feature '{FeatureFlagCode}' is not enabled.");

            await next(context);
        });
    }
}