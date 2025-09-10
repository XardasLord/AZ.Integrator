﻿using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;

public class AssignInvoiceInAllegroJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public string InvoiceNumber { get; set; }
    public string ExternalInvoiceId { get; set; }
    public string TenantId { get; set; }
}