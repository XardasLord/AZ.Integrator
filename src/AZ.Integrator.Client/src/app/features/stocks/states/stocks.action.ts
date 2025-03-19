import { PageEvent } from '@angular/material/paginator';

const prefix = '[Stocks]';

export class LoadStocks {
  static readonly type = `${prefix} ${LoadStocks.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}
