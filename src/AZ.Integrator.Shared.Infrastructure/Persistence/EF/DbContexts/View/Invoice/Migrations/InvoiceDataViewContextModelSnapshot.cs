﻿// <auto-generated />
using System;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Migrations
{
    [DbContext(typeof(InvoiceDataViewContext))]
    partial class InvoiceDataViewContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.InvoiceViewModel", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ExternalOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("external_order_number");

                    b.Property<long>("InvoiceId")
                        .HasColumnType("bigint")
                        .HasColumnName("external_id");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.ToTable((string)null);

                    b.ToView("invoices_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}