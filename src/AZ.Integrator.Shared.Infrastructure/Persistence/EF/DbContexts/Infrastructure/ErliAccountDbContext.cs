using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Infrastructure;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure;

public class ErliAccountDbContext(DbContextOptions<ErliAccountDbContext> options) : DbContext(options)
{
    public virtual DbSet<ErliAccountViewModel> ErliAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ErliAccountConfiguration());
    }
}