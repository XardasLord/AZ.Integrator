const prefix = '[Barcode Scanner]';

export class LoadLogs {
  static readonly type = `${prefix} ${LoadLogs.name}`;
}

export class IncreaseStock {
  static readonly type = `${prefix} ${IncreaseStock.name}`;

  constructor(
    public barcode: string,
    public changeQuantity: number
  ) {}
}

export class DecreaseStock {
  static readonly type = `${prefix} ${DecreaseStock.name}`;

  constructor(
    public barcode: string,
    public changeQuantity: number
  ) {}
}

export class RevertScan {
  static readonly type = `${prefix} ${RevertScan.name}`;

  constructor(
    public barcode: string,
    public changeQuantity: number,
    public scanLogId: number
  ) {}
}
