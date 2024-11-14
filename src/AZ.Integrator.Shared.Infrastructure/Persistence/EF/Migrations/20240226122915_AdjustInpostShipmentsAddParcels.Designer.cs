﻿// <auto-generated />
using System;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Identity.Migrations
{
    [DbContext(typeof(ShipmentDbContext))]
    [Migration("20240226122915_AdjustInpostShipmentsAddParcels")]
    partial class AdjustInpostShipmentsAddParcels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdPackage", b =>
                {
                    b.Property<long>("Number")
                        .HasColumnType("bigint")
                        .HasColumnName("number");

                    b.Property<long>("_shipmentId")
                        .HasColumnType("bigint")
                        .HasColumnName("shipment_id");

                    b.HasKey("Number");

                    b.HasIndex("_shipmentId");

                    b.ToTable("dpd_packages", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdParcel", b =>
                {
                    b.Property<long>("Number")
                        .HasColumnType("bigint")
                        .HasColumnName("number");

                    b.Property<string>("Waybill")
                        .HasColumnType("text")
                        .HasColumnName("waybill");

                    b.Property<long>("_packageId")
                        .HasColumnType("bigint")
                        .HasColumnName("package_id");

                    b.HasKey("Number");

                    b.HasIndex("_packageId");

                    b.ToTable("dpd_parcels", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdShipment", b =>
                {
                    b.Property<long>("SessionNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("session_id");

                    b.Property<string>("AllegroAllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.HasKey("SessionNumber");

                    b.ToTable("dpd_shipments", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.InpostShipment", b =>
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

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Parcel", b =>
                {
                    b.Property<string>("TrackingNumber")
                        .HasColumnType("text")
                        .HasColumnName("tracking_number");

                    b.Property<string>("_shipmentNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("shipment_number");

                    b.HasKey("TrackingNumber");

                    b.HasIndex("_shipmentNumber");

                    b.ToTable("inpost_parcels", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdPackage", b =>
                {
                    b.HasOne("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdShipment", null)
                        .WithMany("Packages")
                        .HasForeignKey("_shipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdParcel", b =>
                {
                    b.HasOne("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdPackage", null)
                        .WithMany("Parcels")
                        .HasForeignKey("_packageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdShipment", b =>
                {
                    b.OwnsOne("AZ.Integrator.Domain.SharedKernel.ValueObjects.CreationInformation", "CreationInformation", b1 =>
                        {
                            b1.Property<long>("DpdShipmentSessionNumber")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("created_at");

                            b1.Property<Guid>("CreatedBy")
                                .HasColumnType("uuid")
                                .HasColumnName("created_by");

                            b1.HasKey("DpdShipmentSessionNumber");

                            b1.ToTable("dpd_shipments");

                            b1.WithOwner()
                                .HasForeignKey("DpdShipmentSessionNumber");
                        });

                    b.Navigation("CreationInformation")
                        .IsRequired();
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.InpostShipment", b =>
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

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Parcel", b =>
                {
                    b.HasOne("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.InpostShipment", null)
                        .WithMany("Parcels")
                        .HasForeignKey("_shipmentNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdPackage", b =>
                {
                    b.Navigation("Parcels");
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdShipment", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.InpostShipment", b =>
                {
                    b.Navigation("Parcels");
                });
#pragma warning restore 612, 618
        }
    }
}
