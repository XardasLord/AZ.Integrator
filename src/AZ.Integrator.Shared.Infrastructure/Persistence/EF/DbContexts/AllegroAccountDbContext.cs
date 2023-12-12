using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Infrastructure;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;

public class AllegroAccountDbContext : DbContext
{
    public virtual DbSet<AllegroAccountViewModel> AllegroAccounts { get; set; }
    
    public AllegroAccountDbContext(DbContextOptions<AllegroAccountDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroAccountConfiguration());
    }
}