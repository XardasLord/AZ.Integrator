﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;

public class UserDbContext : IdentityUserContext<IdentityUser>
{
    public UserDbContext (DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Identity");
        base.OnModelCreating(modelBuilder);
    }
}