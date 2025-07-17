import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { SharedModule } from '../../../../shared/shared.module';
import { MatDialog } from '@angular/material/dialog';
import { StockGroupFormDialogComponent } from '../stock-group-form-dialog/stock-group-form-dialog.component';
import { StockGroupFormDialogResponseModel } from '../stock-group-form-dialog/stock-group-form-dialog-response.model';
import { AddStockGroup } from '../../states/stocks.action';

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

  addStockGroup() {
    const dialogRef = this.dialog.open(StockGroupFormDialogComponent, {
      width: '400px',
      data: null,
    });

    dialogRef.afterClosed().subscribe((result: StockGroupFormDialogResponseModel) => {
      if (!result) {
        return;
      }

      this.store.dispatch(new AddStockGroup(result.name, result.description));
    });
  }
}
