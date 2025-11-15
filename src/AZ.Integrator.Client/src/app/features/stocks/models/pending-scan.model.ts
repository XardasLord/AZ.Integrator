export enum ScanStatus {
  PENDING = 'PENDING',
  SYNCING = 'SYNCING',
  SYNCED = 'SYNCED',
  FAILED = 'FAILED',
}

export enum ScanType {
  IN = 'IN',
  OUT = 'OUT',
}

export interface PendingScan {
  id: string; // UUID lokalny
  barcode: string;
  type: ScanType;
  changeQuantity: number;
  status: ScanStatus;
  timestamp: number;
  retryCount: number;
  lastError?: string;
  scanLogId?: number; // ID z API (po pomyślnej synchronizacji)
}
