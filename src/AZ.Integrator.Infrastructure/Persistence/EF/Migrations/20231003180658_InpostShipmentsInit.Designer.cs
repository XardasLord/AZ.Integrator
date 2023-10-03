﻿// <auto-generated />
using System;
using AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Infrastructure.Persistence.EF.Migrations
{
    [DbContext(typeof(ShipmentDbContext))]
    [Migration("20231003180658_InpostShipmentsInit")]
    partial class InpostShipmentsInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Domain.Aggregates.InpostShipment.InpostShipment", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.Property<string>("AllegroAllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.HasKey("Number");

                    b.ToTable("inpost_shipments", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Domain.Aggregates.InpostShipment.InpostShipment", b =>
                {
                    b.OwnsOne("AZ.Integrator.Domain.SharedKernel.ValueObjects.CreationInformation", "CreationInformation", b1 =>
                        {
                            b1.Property<string>("InpostShipmentNumber")
                                .HasColumnType("text");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("created_at");

                            b1.Property<Guid>("CreatedBy")
                                .HasColumnType("uuid")
                                .HasColumnName("created_by");

                            b1.HasKey("InpostShipmentNumber");

                            b1.ToTable("inpost_shipments");

                            b1.WithOwner()
                                .HasForeignKey("InpostShipmentNumber");
                        });

                    b.Navigation("CreationInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
