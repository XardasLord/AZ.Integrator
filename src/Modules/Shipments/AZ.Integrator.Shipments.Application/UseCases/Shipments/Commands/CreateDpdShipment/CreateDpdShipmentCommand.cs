﻿using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Shared.Application;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.Dpd.Models;
using AZ.Integrator.Shipments.Application.Common.ExternalServices.ShipX.Models;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;

public record CreateDpdShipmentCommand(
    Sender Sender,
    Receiver Receiver,
    List<Parcel> Parcels,
    Insurance Insurance,
    Cod Cod,
    string Reference,
    string Comments,
    string ExternalCustomerId,
    string AllegroOrderId, 
    ShopProviderType? ShopProviderType = null,
    string TenantId = null) : HeaderRequest, IRequest<CreateDpdShipmentResponse>;