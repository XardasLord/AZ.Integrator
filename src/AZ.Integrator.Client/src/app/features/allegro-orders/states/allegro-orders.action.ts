import { PageEvent } from '@angular/material/paginator';
import { CreateShipmentCommand } from '../models/commands/create-shipment.command';
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

export class LoadShipments {
  constructor(public allegroOrderIds: string[] = []) {}

  static readonly type = `${prefix} ${LoadShipments.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class OpenRegisterInPostShipmentModal {
  static readonly type = `${prefix} ${OpenRegisterInPostShipmentModal.name}`;

  constructor(public order: AllegroOrderDetailsModel) {}
}

export class OpenRegisterDpdShipmentModal {
  static readonly type = `${prefix} ${OpenRegisterDpdShipmentModal.name}`;

  constructor(public order: AllegroOrderDetailsModel) {}
}

export class RegisterInpostShipment {
  static readonly type = `${prefix} ${RegisterInpostShipment.name}`;

  constructor(public command: CreateShipmentCommand) {}
}

export class RegisterDpdShipment {
  static readonly type = `${prefix} ${RegisterDpdShipment.name}`;

  constructor(public command: CreateShipmentCommand) {}
}

export class GenerateInpostLabel {
  static readonly type = `${prefix} ${GenerateInpostLabel.name}`;

  constructor(public allegroOrderNumber: string) {}
}

export class GenerateDpdLabel {
  static readonly type = `${prefix} ${GenerateDpdLabel.name}`;

  constructor(public allegroOrderNumber: string) {}
}
