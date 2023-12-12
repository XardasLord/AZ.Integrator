using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;

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