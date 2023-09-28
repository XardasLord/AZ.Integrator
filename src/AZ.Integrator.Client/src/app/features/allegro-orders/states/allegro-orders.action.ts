import { PageEvent } from '@angular/material/paginator';
import { AllegroOrderModel } from '../models/allegro-order.model';

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
