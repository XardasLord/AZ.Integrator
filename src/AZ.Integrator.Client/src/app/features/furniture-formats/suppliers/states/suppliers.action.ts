import { PageEvent } from '@angular/material/paginator';
import { SaveSupplierCommand } from '../models/commands/save-supplier.command';

const prefix = '[Suppliers]';

export class LoadSuppliers {
  static readonly type = `${prefix} ${LoadSuppliers.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}

export class AddSupplier {
  static readonly type = `${prefix} ${AddSupplier.name}`;

  constructor(public command: SaveSupplierCommand) {}
}

export class UpdateSupplier {
  static readonly type = `${prefix} ${UpdateSupplier.name}`;

  constructor(public command: SaveSupplierCommand) {}
}

export class DeleteSupplier {
  static readonly type = `${prefix} ${DeleteSupplier.name}`;

  constructor(public supplierId: number) {}
}
