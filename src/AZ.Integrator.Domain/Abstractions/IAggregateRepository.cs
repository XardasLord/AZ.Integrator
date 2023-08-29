using Ardalis.Specification;
using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Domain.Abstractions;

public interface IAggregateRepository<T>: IRepositoryBase<T> where T : class, IAggregateRoot { }