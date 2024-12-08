using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount;

public class AllegroAccountDataViewContext : DbContext
{
    public virtual DbSet<AllegroAccountViewModel> AllegroAccounts { get; set; }
    
    public AllegroAccountDataViewContext(DbContextOptions<AllegroAccountDataViewContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroAccountViewModelConfiguration());
    }
}