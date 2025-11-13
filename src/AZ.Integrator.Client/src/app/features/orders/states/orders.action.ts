import { PageEvent } from '@angular/material/paginator';
import { CreateShipmentCommand } from '../models/commands/create-shipment.command';
import { OrderDetailsModel } from '../models/order-details.model';

const prefix = '[Orders]';

export class LoadOrders {
  static readonly type = `${prefix} ${LoadOrders.name}`;
}

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
  constructor(public orderIds: string[] = []) {}

  static readonly type = `${prefix} ${LoadShipments.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}

export class SetCurrentTab {
  static readonly type = `${prefix} ${SetCurrentTab.name}`;

  constructor(public currentTab: 'New' | 'ReadyForShipment' | 'Sent') {}
}

export class OpenRegisterDpdShipmentModal {
  static readonly type = `${prefix} ${OpenRegisterDpdShipmentModal.name}`;

  constructor(public order: OrderDetailsModel) {}
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

  constructor(public orderNumber: string) {}
}

export class GenerateInpostLabels {
  static readonly type = `${prefix} ${GenerateInpostLabels.name}`;

  constructor(public orderNumbers: string[]) {}
}

export class GenerateDpdLabel {
  static readonly type = `${prefix} ${GenerateDpdLabel.name}`;

  constructor(public orderNumber: string) {}
}
