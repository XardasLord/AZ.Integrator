import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { LoadStockGroups, LoadStocks, UpdateStockGroup } from '../../states/stocks.action';
import { StockGroupViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StocksState } from '../../states/stocks.state';
import { environment } from '../../../../../environments/environment';
import { StockGroupFormDialogComponent } from '../../components/stock-group-form-dialog/stock-group-form-dialog.component';
import { StockGroupFormDialogResponseModel } from '../../components/stock-group-form-dialog/stock-group-form-dialog-response.model';

@Component({
  selector: 'app-stocks-list',
  imports: [SharedModule],
  templateUrl: './stocks-list.component.html',
  styleUrl: './stocks-list.component.scss',
  standalone: true,
})
export class StocksListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  stockWarningThreshold = environment.stockWarningThreshold;
  groups$: Observable<StockGroupViewModel[]> = this.store.select(StocksState.groupedStocks);

  ngOnInit(): void {
    this.store.dispatch([new LoadStocks(), new LoadStockGroups()]);
  }

  hasWarning(group: StockGroupViewModel): boolean {
    return group.stocks?.some(s => s.quantity < this.stockWarningThreshold);
  }

  editGroup(group: StockGroupViewModel) {
    const dialogRef = this.dialog.open(StockGroupFormDialogComponent, {
      width: '400px',
      data: {
        name: group.name,
        description: group.description,
      },
    });

    dialogRef.afterClosed().subscribe((result: StockGroupFormDialogResponseModel) => {
      if (!result) {
        return;
      }

      this.store.dispatch(new UpdateStockGroup(group.id, result.name, result.description));
    });
  }
}
