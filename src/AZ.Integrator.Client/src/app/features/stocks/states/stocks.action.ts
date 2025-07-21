import { PageEvent } from '@angular/material/paginator';

const prefix = '[Stocks]';

export class LoadStocks {
  static readonly type = `${prefix} ${LoadStocks.name}`;
}

export class LoadStockGroups {
  static readonly type = `${prefix} ${LoadStockGroups.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}

export class AddStockGroup {
  static readonly type = `${prefix} ${AddStockGroup.name}`;

  constructor(
    public name: string,
    public description: string
  ) {}
}

export class UpdateStockGroup {
  static readonly type = `${prefix} ${UpdateStockGroup.name}`;

  constructor(
    public groupId: number,
    public name: string,
    public description: string
  ) {}
}

export class ChangeGroup {
  static readonly type = `${prefix} ${ChangeGroup.name}`;

  constructor(
    public packageCode: string,
    public newGroupId: number
  ) {}
}

export class ChangeThreshold {
  static readonly type = `${prefix} ${ChangeThreshold.name}`;

  constructor(
    public packageCode: string,
    public threshold: number
  ) {}
}
