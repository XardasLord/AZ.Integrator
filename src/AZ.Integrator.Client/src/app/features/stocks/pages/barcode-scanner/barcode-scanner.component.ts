import { AfterViewInit, Component, ElementRef, inject, Input, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { DecreaseStock, IncreaseStock, LoadLogs } from '../../states/barcode-scanner.action';
import { StockLogViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { BarcodeScannerState } from '../../states/barcode-scanner.state';

export type BarcodeScannerType = 'in' | 'out';

@Component({
  selector: 'app-barcode-scanner',
  imports: [SharedModule],
  templateUrl: './barcode-scanner.component.html',
  styleUrl: './barcode-scanner.component.scss',
  standalone: true,
})
export class BarcodeScannerComponent implements OnInit, AfterViewInit {
  private store = inject(Store);
  @Input() type!: BarcodeScannerType;
  @ViewChild('barcodeInput') barcodeInput!: ElementRef;

  barcode: string = '';
  barcodeScannerLogs$: Observable<StockLogViewModel[]> = this.store.select(BarcodeScannerState.logs);

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());
  }

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
