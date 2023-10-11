import { PageEvent } from '@angular/material/paginator';
import { CreateInpostShipmentCommand } from '../models/commands/create-inpost-shipment.command';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';

const prefix = '[Allegro Orders]';

export class LoadNew {
  static readonly type = `${prefix} ${LoadNew.name}`;
}

export class LoadReadyForShipment {
  static readonly type = `${prefix} ${LoadReadyForShipment.name}`;
}

export class LoadSent {
  static readonly type = `${prefix} ${LoadSent.name}`;
}

export class LoadInpostShipments {
  static readonly type = `${prefix} ${LoadInpostShipments.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class OpenRegisterInPostShipmentModal {
  static readonly type = `${prefix} ${OpenRegisterInPostShipmentModal.name}`;

  constructor(public order: AllegroOrderDetailsModel) {}
}

export class RegisterInpostShipment {
  static readonly type = `${prefix} ${RegisterInpostShipment.name}`;

  constructor(public command: CreateInpostShipmentCommand) {}
}

export class GenerateInpostLabel {
  static readonly type = `${prefix} ${GenerateInpostLabel.name}`;

  constructor(public allegroOrderNumber: string) {}
}
