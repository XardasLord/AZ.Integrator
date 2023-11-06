const prefix = '[Invoices]';

export class LoadInvoices {
  constructor(public allegroOrderIds: string[] = []) {}

  static readonly type = `${prefix} ${LoadInvoices.name}`;
}

export class GenerateInvoice {
  static readonly type = `${prefix} ${GenerateInvoice.name}`;

  constructor(public allegroOrderNumber: string) {}
}
