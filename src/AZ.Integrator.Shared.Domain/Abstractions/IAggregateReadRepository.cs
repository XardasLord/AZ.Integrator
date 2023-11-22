using Ardalis.Specification;
using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Domain.Abstractions;

public interface IAggregateReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }