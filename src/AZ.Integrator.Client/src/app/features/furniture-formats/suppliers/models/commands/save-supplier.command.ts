export interface SaveSupplierCommand {
  id?: number | null;
  name: string;
  telephoneNumber: string;
  mailboxes: SupplierMailboxDto[];
}

export interface SupplierMailboxDto {
  id?: number | null;
  email: string;
}
