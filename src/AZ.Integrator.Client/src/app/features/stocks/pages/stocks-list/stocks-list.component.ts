import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CdkDrag, CdkDragDrop, CdkDropList, transferArrayItem } from '@angular/cdk/drag-drop';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SharedModule } from '../../../../shared/shared.module';
import {
  ChangeGroup,
  ChangeThreshold,
  LoadStockGroups,
  LoadStocks,
  UpdateStockGroup,
} from '../../states/stocks.action';
import { StockGroupViewModel, StockViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StocksState } from '../../states/stocks.state';
import { StockGroupFormDialogComponent } from '../../components/stock-group-form-dialog/stock-group-form-dialog.component';
import { StockGroupFormDialogResponseModel } from '../../components/stock-group-form-dialog/stock-group-form-dialog-response.model';
import { StockThresholdFormDialogComponent } from '../../components/stock-threshold-form-dialog/stock-threshold-form-dialog.component';
import { StockThresholdFormDialogModel } from '../../components/stock-threshold-form-dialog/stock-threshold-form-dialog.model';
import { StockGroupFormDialogModel } from '../../components/stock-group-form-dialog/stock-group-form-dialog.model';
import { StockThresholdFormDialogResponseModel } from '../../components/stock-threshold-form-dialog/stock-threshold-form-dialog-response.model';

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
  private destroyRef = inject(DestroyRef);

  groups$: Observable<StockGroupViewModel[]> = this.store.select(StocksState.groupedStocks);
  connectedDropLists$: Observable<string[]> = this.groups$.pipe(map(groups => groups.map(g => `group-${g.id}`)));

  protected readonly Math = Math;

  ngOnInit(): void {
    this.store.dispatch([new LoadStocks(), new LoadStockGroups()]);
  }

  editGroup(group: StockGroupViewModel) {
    const dialogRef = this.dialog.open(StockGroupFormDialogComponent, {
      width: '400px',
      data: <StockGroupFormDialogModel>{
        name: group.name,
        description: group.description,
      },
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((result: StockGroupFormDialogResponseModel) => {
        if (!result) {
          return;
        }

        this.store.dispatch(new UpdateStockGroup(group.id, result.name, result.description));
      });
  }

  editThreshold(stock: StockViewModel) {
    const dialogRef = this.dialog.open(StockThresholdFormDialogComponent, {
      width: '400px',
      data: <StockThresholdFormDialogModel>{
        threshold: stock.threshold,
        packageCode: stock.packageCode,
      },
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((result: StockThresholdFormDialogResponseModel) => {
        if (!result) {
          return;
        }

        this.store.dispatch(new ChangeThreshold(stock.packageCode, result.threshold));
      });
  }

  onDropPackageCodeToGroup(event: CdkDragDrop<StockViewModel[]>, targetGroupId: number) {
    if (event.previousContainer === event.container) return;

    const stock = event.previousContainer.data[event.previousIndex];

    transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);

    this.store.dispatch(new ChangeGroup(stock.packageCode, targetGroupId));
  }

  hasAnyWarning(group: StockGroupViewModel): boolean {
    return group.stocks?.some(stock => this.isBelowThreshold(stock));
  }

  getDifferenceBetweenThreshold(stock: StockViewModel): number {
    return stock.quantity - stock.threshold;
  }

  isBelowThreshold(stock: StockViewModel): boolean {
    return stock.quantity < stock.threshold;
  }

  isAboveThreshold(stock: StockViewModel): boolean {
    return stock.quantity > stock.threshold;
  }

  getStockQuantityColor(stock: StockViewModel) {
    const ratio = stock.quantity / stock.threshold;
    if (ratio < 0.5) return 'bg-red-100 text-red-700';
    if (ratio < 1) return 'bg-orange-100 text-orange-700';
    return 'bg-green-100 text-green-700';
  }

  getStockQuantityCardColor(stock: StockViewModel) {
    const ratio = stock.quantity / stock.threshold;
    if (ratio < 0.5) return '!from-red-50 !to-red-100';
    if (ratio < 1) return '!from-orange-50 !to-orange-100';
    return '!from-green-50 !to-green-100';
  }

  getDifferenceIcon(stock: StockViewModel): string {
    const diff = this.getDifferenceBetweenThreshold(stock);
    if (diff > 0) return 'arrow_upward';
    if (diff < 0) return 'arrow_downward';
    return '';
  }

  getDifferenceClass(stock: StockViewModel): string {
    const diff = this.getDifferenceBetweenThreshold(stock);
    if (diff > 0) return '!text-green-600';
    if (diff < 0) return '!text-red-600';
    return '';
  }

  trackByGroupId(index: number, group: StockGroupViewModel): number {
    return group.id;
  }

  trackByStockCode(index: number, stock: StockViewModel): string {
    return stock.packageCode;
  }
}
