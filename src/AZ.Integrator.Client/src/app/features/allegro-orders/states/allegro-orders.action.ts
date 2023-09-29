import { PageEvent } from '@angular/material/paginator';
import { AllegroOrderModel } from '../models/allegro-order.model';
import { CreateInpostShipmentCommand } from '../models/commands/create-inpost-shipment.command';

const prefix = '[Allegro Orders]';

export class Load {
  static readonly type = `${prefix} ${Load.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class OpenRegisterParcelModal {
  static readonly type = `${prefix} ${OpenRegisterParcelModal.name}`;

  constructor(public order: AllegroOrderModel) {}
}

export class RegisterInpostShipment {
  static readonly type = `${prefix} ${RegisterInpostShipment.name}`;

  constructor(public command: CreateInpostShipmentCommand) {}
}
