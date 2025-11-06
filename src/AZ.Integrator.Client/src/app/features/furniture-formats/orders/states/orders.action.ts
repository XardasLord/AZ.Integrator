import { PageEvent } from '@angular/material/paginator';
import { CreateFurnitureLineRequest } from '../models/part-line-form-group.model';

const prefix = '[Part Definition Orders]';

export class LoadOrders {
  static readonly type = `${prefix} ${LoadOrders.name}`;
}

export class CreateOrder {
  static readonly type = `${prefix} ${CreateOrder.name}`;

  constructor(
    public supplierId: number,
    public furnitureLineRequests: CreateFurnitureLineRequest[]
  ) {}
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;
  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchText: string) {}
}
