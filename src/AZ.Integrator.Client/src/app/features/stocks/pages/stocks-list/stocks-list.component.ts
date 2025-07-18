import { Component, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CdkDragDrop, transferArrayItem, CdkDropList, CdkDrag } from '@angular/cdk/drag-drop';
import { Store } from '@ngxs/store';
import { Observable, map } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { ChangeGroup, LoadStockGroups, LoadStocks, UpdateStockGroup } from '../../states/stocks.action';
import { StockGroupViewModel, StockViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StocksState } from '../../states/stocks.state';
import { environment } from '../../../../../environments/environment';
import { StockGroupFormDialogComponent } from '../../components/stock-group-form-dialog/stock-group-form-dialog.component';
import { StockGroupFormDialogResponseModel } from '../../components/stock-group-form-dialog/stock-group-form-dialog-response.model';

@Component({
  selector: 'app-stocks-list',
  imports: [SharedModule, CdkDropList, CdkDrag],
  templateUrl: './stocks-list.component.html',
  styleUrl: './stocks-list.component.scss',
  standalone: true,
})
export class StocksListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  stockWarningThreshold = environment.stockWarningThreshold;
  groups$: Observable<StockGroupViewModel[]> = this.store.select(StocksState.groupedStocks);
  connectedDropLists$: Observable<string[]> = this.groups$.pipe(
    map(groups => groups.map(g => `group-${g.id}`))
  );

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

  onDropPackageCodeToGroup(event: CdkDragDrop<StockViewModel[]>, targetGroupId: number) {
    if (event.previousContainer === event.container) return;
    const stock = event.previousContainer.data[event.previousIndex];
    transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);
    this.store.dispatch(new ChangeGroup(stock.packageCode, targetGroupId));
  }

  trackByGroupId(index: number, group: StockGroupViewModel): number {
    return group.id;
  }

  trackByStockCode(index: number, stock: StockViewModel): string {
    return stock.packageCode;
  }
}
