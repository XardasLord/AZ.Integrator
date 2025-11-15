import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SharedModule } from '../../../../shared/shared.module';
import { ScanStatus } from '../../models/pending-scan.model';
import { BarcodeScannerState } from '../../states/barcode-scanner.state';

@Component({
  selector: 'app-scan-status-indicator',
  imports: [SharedModule],
  template: `
    <div class="flex items-center justify-center gap-3 py-2 px-3 bg-white rounded-lg shadow-sm border">
      <!-- Pending -->
      <div class="flex items-center gap-1.5">
        <mat-icon class="!text-yellow-600 text-base">schedule</mat-icon>
        <span class="text-sm font-medium text-gray-700">{{ pendingCount$ | async }}</span>
      </div>

      <!-- Syncing -->
      <div class="flex items-center gap-1.5">
        <mat-icon class="!text-blue-600 text-base animate-spin">sync</mat-icon>
        <span class="text-sm font-medium text-gray-700">{{ syncingCount$ | async }}</span>
      </div>

      <!-- Synced -->
      <div class="flex items-center gap-1.5">
        <mat-icon class="!text-green-600 text-base">check_circle</mat-icon>
        <span class="text-sm font-medium text-gray-700">{{ syncedCount$ | async }}</span>
      </div>

      <!-- Failed -->
      <div class="flex items-center gap-1.5">
        <mat-icon class="!text-red-600 text-base">error</mat-icon>
        <span class="text-sm font-medium text-gray-700">{{ failedCount$ | async }}</span>
      </div>
    </div>
  `,
  styles: [
    `
      @keyframes spin {
        from {
          transform: rotate(0deg);
        }
        to {
          transform: rotate(360deg);
        }
      }
      .animate-spin {
        animation: spin 1s linear infinite;
      }
    `,
  ],
  standalone: true,
})
export class ScanStatusIndicatorComponent {
  private store = inject(Store);

  pendingCount$: Observable<number> = this.store
    .select(BarcodeScannerState.pendingScans)
    .pipe(map(scans => scans.filter(s => s.status === ScanStatus.PENDING).length));

  syncingCount$: Observable<number> = this.store
    .select(BarcodeScannerState.pendingScans)
    .pipe(map(scans => scans.filter(s => s.status === ScanStatus.SYNCING).length));

  syncedCount$: Observable<number> = this.store
    .select(BarcodeScannerState.pendingScans)
    .pipe(map(scans => scans.filter(s => s.status === ScanStatus.SYNCED).length));

  failedCount$: Observable<number> = this.store
    .select(BarcodeScannerState.pendingScans)
    .pipe(map(scans => scans.filter(s => s.status === ScanStatus.FAILED).length));
}
