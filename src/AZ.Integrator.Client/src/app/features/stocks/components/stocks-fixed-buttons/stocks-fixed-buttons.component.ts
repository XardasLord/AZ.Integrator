import { Component, DestroyRef, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { Store } from '@ngxs/store';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { SharedModule } from '../../../../shared/shared.module';
import { StockGroupFormDialogComponent } from '../stock-group-form-dialog/stock-group-form-dialog.component';
import { StockGroupFormDialogResponseModel } from '../stock-group-form-dialog/stock-group-form-dialog-response.model';
import { AddStockGroup, LoadStockGroups, LoadStocks } from '../../states/stocks.action';

@Component({
  selector: 'app-stocks-fixed-buttons',
  imports: [SharedModule],
  templateUrl: './stocks-fixed-buttons.component.html',
  styleUrl: './stocks-fixed-buttons.component.scss',
  standalone: true,
})
export class StocksFixedButtonsComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  addStockGroup() {
    const dialogRef = this.dialog.open(StockGroupFormDialogComponent, {
      width: '400px',
      data: null,
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((result: StockGroupFormDialogResponseModel) => {
        if (!result) {
          return;
        }

        this.store.dispatch(new AddStockGroup(result.name, result.description));
      });
  }

  refreshStocks() {
    this.store.dispatch([new LoadStocks(), new LoadStockGroups()]);
  }
}
