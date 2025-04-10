﻿// <auto-generated />

#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Migrations
{
    [DbContext(typeof(InvoiceDataViewContext))]
    [Migration("20231103092741_InvoicesViewInit")]
    partial class InvoicesViewInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels.InvoiceViewModel", b =>
                {
                    b.Property<string>("AllegroOrderNumber")
                        .HasColumnType("text")
                        .HasColumnName("allegro_order_number");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("text")
                        .HasColumnName("invoice_number");

                    b.ToTable((string)null);

                    b.ToView("invoices_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
