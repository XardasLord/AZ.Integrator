import { Component, inject, Input, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { LoadLogs } from '../../states/barcode-scanner.action';
import { StockLogViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { BarcodeScannerState } from '../../states/barcode-scanner.state';
import { BarcodeScannerType } from '../barcode-scanner/barcode-scanner.component';

@Component({
  selector: 'app-barcode-scanned-codes-list',
  imports: [SharedModule],
  templateUrl: './barcode-scanned-codes-list.component.html',
  styleUrl: './barcode-scanned-codes-list.component.scss',
  standalone: true,
})
export class BarcodeScannedCodesListComponent implements OnInit {
  private store = inject(Store);
  @Input() type!: BarcodeScannerType;

  barcodeScannerLogs$: Observable<StockLogViewModel[]> = this.store.select(BarcodeScannerState.logs);

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());
  }
}
