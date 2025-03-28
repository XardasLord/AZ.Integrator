﻿// <auto-generated />
using System;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment.Migrations
{
    [DbContext(typeof(ShipmentDataViewContext))]
    partial class ShipmentDataViewContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.DpdShipmentViewModel", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExternalOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("external_order_number");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("session_id");

                    b.ToTable((string)null);

                    b.ToView("dpd_shipments_view", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.InpostShipmentViewModel", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExternalOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("external_order_number");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.ToTable((string)null);

                    b.ToView("inpost_shipments_view", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.ShipmentViewModel", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExternalOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("external_order_number");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("shipment_number");

                    b.Property<string>("ShipmentProvider")
                        .HasColumnType("text")
                        .HasColumnName("shipment_provider");

                    b.ToTable((string)null);

                    b.ToView("shipments_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
