import { AfterViewInit, Component, ElementRef, inject, Input, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { SharedModule } from '../../../../shared/shared.module';
import { DecreaseStock, IncreaseStock } from '../../states/barcode-scanner.action';
import { BarcodeScannedCodesListComponent } from '../barcode-scanned-codes-list/barcode-scanned-codes-list.component';

export type BarcodeScannerType = 'in' | 'out';

@Component({
  selector: 'app-barcode-scanner',
  imports: [SharedModule, BarcodeScannedCodesListComponent],
  templateUrl: './barcode-scanner.component.html',
  styleUrl: './barcode-scanner.component.scss',
  standalone: true,
})
export class BarcodeScannerComponent implements AfterViewInit {
  private store = inject(Store);
  @Input() type!: BarcodeScannerType;
  @ViewChild('barcodeInput') barcodeInput!: ElementRef;

  barcode: string = '';

  ngAfterViewInit(): void {
    this.focusInput();
  }

  scanBarcode() {
    if (!this.barcode.trim()) {
      return;
    }

    if (this.type === 'in') {
      this.increaseStock();
    } else {
      this.decreaseStock();
    }

    this.barcode = '';
    this.focusInput();
  }

  private focusInput() {
    setTimeout(() => this.barcodeInput.nativeElement.focus(), 100);
  }

  private increaseStock() {
    this.store.dispatch(new IncreaseStock(this.barcode.trim(), 1));
  }

  private decreaseStock() {
    this.store.dispatch(new DecreaseStock(this.barcode.trim(), -1));
  }
}
