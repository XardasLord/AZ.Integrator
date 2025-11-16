import { ScanType } from '../models/pending-scan.model';

const prefix = '[Barcode Scanner]';

export class LoadLogs {
  static readonly type = `${prefix} ${LoadLogs.name}`;
}

export class LoadPendingScans {
  static readonly type = `${prefix} ${LoadPendingScans.name}`;
}

export class AddScan {
  static readonly type = `${prefix} ${AddScan.name}`;

  constructor(
    public barcode: string,
    public type: ScanType
  ) {}
}

export class RemoveScan {
  static readonly type = `${prefix} ${RemoveScan.name}`;

  constructor(public scanId: string) {}
}

export class ProcessQueue {
  static readonly type = `${prefix} ${ProcessQueue.name}`;
}

export class ProcessSingleScan {
  static readonly type = `${prefix} ${ProcessSingleScan.name}`;

  constructor(public scanId: string) {}
}

export class RetryFailed {
  static readonly type = `${prefix} ${RetryFailed.name}`;
}

export class ClearSynced {
  static readonly type = `${prefix} ${ClearSynced.name}`;
}

export class RevertScan {
  static readonly type = `${prefix} ${RevertScan.name}`;

  constructor(public scanId: string) {}
}

export class ResetCounters {
  static readonly type = `${prefix} ${ResetCounters.name}`;
}
