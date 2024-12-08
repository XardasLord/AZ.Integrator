const prefix = '[Invoices]';

export class LoadInvoices {
  constructor(public orderIds: string[] = []) {}

  static readonly type = `${prefix} ${LoadInvoices.name}`;
}

export class GenerateInvoice {
  static readonly type = `${prefix} ${GenerateInvoice.name}`;

  constructor(public orderNumber: string) {}
}

export class DownloadInvoice {
  static readonly type = `${prefix} ${DownloadInvoice.name}`;

  constructor(
    public invoiceId: number,
    public invoiceNumber: string
  ) {}
}
