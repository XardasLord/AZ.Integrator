import { AfterViewInit, Component, ElementRef, inject, Input, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { SharedModule } from '../../../../shared/shared.module';
import { BarcodeScannedCodesListComponent } from '../barcode-scanned-codes-list/barcode-scanned-codes-list.component';
import { ScanStatusIndicatorComponent } from '../../components/scan-status-indicator/scan-status-indicator.component';
import { ScanType } from '../../models/pending-scan.model';
import { AddScan, LoadPendingScans } from '../../states/barcode-scanner.action';

export type BarcodeScannerType = 'in' | 'out';

@Component({
  selector: 'app-barcode-scanner',
  imports: [SharedModule, BarcodeScannedCodesListComponent, ScanStatusIndicatorComponent],
  templateUrl: './barcode-scanner.component.html',
  styleUrl: './barcode-scanner.component.scss',
  standalone: true,
})
export class BarcodeScannerComponent implements OnInit, AfterViewInit {
  private store = inject(Store);

  @Input() type!: BarcodeScannerType;
  @ViewChild('barcodeInput') barcodeInput!: ElementRef;

  barcode: string = '';

  ngOnInit(): void {
    this.store.dispatch(new LoadPendingScans());
  }

  ngAfterViewInit(): void {
    this.focusInput();
  }

  scanBarcode() {
    if (!this.barcode.trim()) {
      return;
    }

    // Dodaj skan do kolejki przez NGXS action
    const scanType = this.type === 'in' ? ScanType.IN : ScanType.OUT;
    this.store.dispatch(new AddScan(this.barcode.trim(), scanType));

    this.barcode = '';
    this.focusInput();
  }

  private focusInput() {
    setTimeout(() => this.barcodeInput.nativeElement.focus(), 200);
  }
}
