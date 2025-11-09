using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount;

public class AllegroAccountDataViewContext(DbContextOptions<AllegroAccountDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<AllegroAccountViewModel> AllegroAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroAccountViewModelConfiguration());
    }
}