﻿using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;

public record RegisterInvoiceCommand(string OrderNumber) : HeaderRequest, IRequest;