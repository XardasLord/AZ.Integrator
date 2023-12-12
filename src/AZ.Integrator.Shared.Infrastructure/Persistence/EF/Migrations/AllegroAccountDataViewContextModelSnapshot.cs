﻿// <auto-generated />
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    [DbContext(typeof(AllegroAccountDataViewContext))]
    partial class AllegroAccountDataViewContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.AllegroAccountViewModel", b =>
                {
                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("access_token");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("tenant_id");

                    b.ToTable((string)null);

                    b.ToView("allegro_accounts_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
