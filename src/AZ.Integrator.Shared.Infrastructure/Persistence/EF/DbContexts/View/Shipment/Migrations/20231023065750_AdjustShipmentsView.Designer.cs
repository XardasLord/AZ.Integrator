﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment.Migrations
{
    [DbContext(typeof(ShipmentDataViewContext))]
    [Migration("20231023065750_AdjustShipmentsView")]
    partial class AdjustShipmentsView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.DpdShipmentViewModel", b =>
                {
                    b.Property<string>("AllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("session_id");

                    b.ToTable((string)null);

                    b.ToView("dpd_shipments_view", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.InpostShipmentViewModel", b =>
                {
                    b.Property<string>("AllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("text")
                        .HasColumnName("tracking_number");

                    b.ToTable((string)null);

                    b.ToView("inpost_shipments_view", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.ShipmentViewModel", b =>
                {
                    b.Property<string>("AllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.Property<int>("ShipmentProvided")
                        .HasColumnType("integer");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("text")
                        .HasColumnName("tracking_number");

                    b.ToTable((string)null);

                    b.ToView("shipments_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
