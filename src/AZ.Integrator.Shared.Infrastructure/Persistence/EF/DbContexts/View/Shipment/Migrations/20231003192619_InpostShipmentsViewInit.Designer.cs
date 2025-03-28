﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment.Migrations
{
    [DbContext(typeof(ShipmentDataViewContext))]
    [Migration("20231003192619_InpostShipmentsViewInit")]
    partial class InpostShipmentsViewInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.InpostShipmentViewModel", b =>
                {
                    b.Property<string>("AllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("InpostShipmentNumber")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.ToTable((string)null);

                    b.ToView("inpost_shipments_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
