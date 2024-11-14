﻿using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Infrastructure;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure;

public class AllegroAccountDbContext(DbContextOptions<AllegroAccountDbContext> options) : DbContext(options)
{
    public virtual DbSet<AllegroAccountViewModel> AllegroAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AllegroAccountConfiguration());
    }
}