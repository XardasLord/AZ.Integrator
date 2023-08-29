using Ardalis.Specification.EntityFrameworkCore;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Repositories;

public class AggregateReadRepository<TAggregate, TDbContext> : RepositoryBase<TAggregate>, IAggregateReadRepository<TAggregate>
    where TAggregate : class, IAggregateRoot
    where TDbContext : DbContext
{
    public AggregateReadRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}