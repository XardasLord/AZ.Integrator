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

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    [DbContext(typeof(InvoiceDbContext))]
    [Migration("20231211074829_AlterInvoicesDbContextAddExternalId")]
    partial class AlterInvoicesDbContextAddExternalId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Invoice", b =>
                {
                    b.Property<int>("ExternalId")
                        .HasColumnType("integer")
                        .HasColumnName("external_id");

                    b.Property<string>("AllegroAllegroOrderNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<string>("Number")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.HasKey("ExternalId");

                    b.ToTable("invoices", (string)null);
                });

            modelBuilder.Entity("AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Invoice", b =>
                {
                    b.OwnsOne("AZ.Integrator.Domain.SharedKernel.ValueObjects.CreationInformation", "CreationInformation", b1 =>
                        {
                            b1.Property<int>("InvoiceExternalId")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("created_at");

                            b1.Property<Guid>("CreatedBy")
                                .HasColumnType("uuid")
                                .HasColumnName("created_by");

                            b1.HasKey("InvoiceExternalId");

                            b1.ToTable("invoices");

                            b1.WithOwner()
                                .HasForeignKey("InvoiceExternalId");
                        });

                    b.Navigation("CreationInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
