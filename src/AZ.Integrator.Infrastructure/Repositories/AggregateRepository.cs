using Ardalis.Specification.EntityFrameworkCore;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Repositories;

public class AggregateRepository<TAggregate, TDbContext> : RepositoryBase<TAggregate>, IAggregateRepository<TAggregate> 
    where TAggregate : class, IAggregateRoot
    where TDbContext : DbContext
{
    private readonly DbContext _dbContext;
    private readonly IMediator _mediator;

    public AggregateRepository(TDbContext dbContext, IMediator mediator) : base(dbContext)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await base.SaveChangesAsync(cancellationToken);
        await _mediator.DispatchDomainEventsAsync(_dbContext);

        return 1;
    }
}