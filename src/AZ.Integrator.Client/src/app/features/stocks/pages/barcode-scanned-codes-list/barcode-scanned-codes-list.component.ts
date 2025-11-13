import { Component, DestroyRef, inject, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { LoadLogs, RevertScan } from '../../states/barcode-scanner.action';
import { StockLogStatus, StockLogViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { BarcodeScannerState } from '../../states/barcode-scanner.state';
import { BarcodeScannerType } from '../barcode-scanner/barcode-scanner.component';
import { ConfirmScanRevertDialogComponent } from '../../components/convert-scan-revert-dialog/confirm-scan-revert-dialog.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-barcode-scanned-codes-list',
  imports: [SharedModule],
  templateUrl: './barcode-scanned-codes-list.component.html',
  styleUrl: './barcode-scanned-codes-list.component.scss',
  standalone: true,
})
export class BarcodeScannedCodesListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  @Input() type!: BarcodeScannerType;

  barcodeScannerLogs$: Observable<StockLogViewModel[]> = this.store.select(BarcodeScannerState.logs);

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());
  }

  confirmRevert(log: StockLogViewModel): void {
    const dialogRef = this.dialog.open(ConfirmScanRevertDialogComponent, {
      data: { packageCode: log.packageCode },
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((confirmed: boolean) => {
        if (confirmed) {
          this.revertScanLog(log);
        }
      });
  }

  revertScanLog(log: StockLogViewModel): void {
    this.store.dispatch(new RevertScan(log.packageCode!, -log.changeQuantity, log.id));
  }

  protected readonly StockLogStatus = StockLogStatus;
}
