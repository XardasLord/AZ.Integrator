using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View;

public class InvoiceDataViewContext : DbContext
{
    public virtual DbSet<InvoiceViewModel> Invoices { get; set; }
    
    public InvoiceDataViewContext(DbContextOptions<InvoiceDataViewContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceViewModelConfiguration());
    }
}