using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View.Configurations;
using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Invoices.Infrastructure.Persistence.EF.View;

public class InvoiceDataViewContext(DbContextOptions<InvoiceDataViewContext> options) : DbContext(options)
{
    public virtual DbSet<InvoiceViewModel> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceViewModelConfiguration());
    }
}