import { Component, inject, Input, OnInit } from '@angular/core';
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
export class BarcodeScannerComponent implements OnInit {
  private store = inject(Store);
  @Input() type!: BarcodeScannerType;

  barcode: string = '';
  barcodeScannerLogs$: Observable<StockLogViewModel[]> = this.store.select(BarcodeScannerState.logs);

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());
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
  }

  private increaseStock() {
    this.store.dispatch(new IncreaseStock(this.barcode.trim(), 1));
  }

  private decreaseStock() {
    this.store.dispatch(new DecreaseStock(this.barcode.trim(), -1));
  }
}
