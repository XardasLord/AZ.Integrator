using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ErliAccount.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ErliAccount;

public class ErliAccountDbContext(DbContextOptions<ErliAccountDbContext> options) : DbContext(options)
{
    public virtual DbSet<ErliAccountViewModel> ErliAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ErliAccountConfiguration());
    }
}