using AZ.Integrator.Domain.SeedWork;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Mediator;

internal static class Extensions
{
    public static async Task DispatchDomainEventsAsync<T>(this IMediator mediator, T dbContext) where T : DbContext
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Events != null && x.Entity.Events.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Events)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async domainEvent => await mediator.Publish(domainEvent));

        await Task.WhenAll(tasks);
    }
}