﻿// <auto-generated />
using System;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    [DbContext(typeof(AllegroAccountDbContext))]
    partial class AllegroAccountDbContextModelSnapshot : ModelSnapshot
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
                    b.Property<string>("TenantId")
                        .HasColumnType("text")
                        .HasColumnName("tenant_id");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("access_token");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_id");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_secret");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("expires_at");

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("redirect_uri");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.HasKey("TenantId");

                    b.ToTable("allegro_accounts", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
