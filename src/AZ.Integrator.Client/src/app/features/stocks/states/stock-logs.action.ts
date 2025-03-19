const prefix = '[Stock Logs]';

export class LoadLogs {
  static readonly type = `${prefix} ${LoadLogs.name}`;
}

export class ApplyFilters {
  static readonly type = `${prefix} ${ApplyFilters.name}`;

  constructor(
    public searchPhrase: string,
    public startDate: Date,
    public endDate: Date
  ) {}
}
