import { Component, DestroyRef, inject, Input } from '@angular/core';
import { Store } from '@ngxs/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SharedModule } from '../../../../shared/shared.module';
import { BarcodeScannerType } from '../barcode-scanner/barcode-scanner.component';
import { PendingScan, ScanStatus, ScanType } from '../../models/pending-scan.model';
import { ConfirmScanRevertDialogComponent } from '../../components/convert-scan-revert-dialog/confirm-scan-revert-dialog.component';
import { BarcodeScannerState } from '../../states/barcode-scanner.state';
import { ClearSynced, RemoveScan, ResetCounters, RetryFailed, RevertScan } from '../../states/barcode-scanner.action';

@Component({
  selector: 'app-barcode-scanned-codes-list',
  imports: [SharedModule],
  templateUrl: './barcode-scanned-codes-list.component.html',
  styleUrl: './barcode-scanned-codes-list.component.scss',
  standalone: true,
})
export class BarcodeScannedCodesListComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  @Input() type!: BarcodeScannerType;

  scans$: Observable<PendingScan[]> = combineLatest([
    this.store.select(BarcodeScannerState.pendingScans),
    this.store.select(BarcodeScannerState.syncedScans),
  ]).pipe(
    map(([pending, synced]) => {
      const allScans = [...(pending || []), ...(synced || [])];
      return allScans.filter(s => (this.type === 'in' ? s.type === ScanType.IN : s.type === ScanType.OUT));
    })
  );

  protected readonly ScanStatus = ScanStatus;

  removeScan(scan: PendingScan): void {
    this.store.dispatch(new RemoveScan(scan.id));
  }

  retryFailed(): void {
    this.store.dispatch(new RetryFailed());
  }

  clearSynced(): void {
    this.store.dispatch(new ClearSynced());
  }

  resetCounters(): void {
    this.store.dispatch(new ResetCounters());
  }

  confirmRevert(scan: PendingScan): void {
    const dialogRef = this.dialog.open(ConfirmScanRevertDialogComponent, {
      data: { packageCode: scan.barcode },
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((confirmed: boolean) => {
        if (confirmed) {
          this.revertScan(scan);
        }
      });
  }

  revertScan(scan: PendingScan): void {
    this.store.dispatch(new RevertScan(scan.id));
  }
}
