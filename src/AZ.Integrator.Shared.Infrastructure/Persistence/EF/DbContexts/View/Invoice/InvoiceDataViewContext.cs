using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice;

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