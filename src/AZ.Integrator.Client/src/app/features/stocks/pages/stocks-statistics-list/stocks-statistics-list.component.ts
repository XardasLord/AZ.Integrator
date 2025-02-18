import { Component, inject, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { StockLogViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StockLogsState } from '../../states/stock-logs.state';
import { LoadLogs } from '../../states/stock-logs.action';

interface UserScanningStats {
  createdBy: string;
  totalScanned: number;
}

@Component({
  selector: 'app-stocks-statistics-list',
  imports: [SharedModule],
  templateUrl: './stocks-statistics-list.component.html',
  styleUrl: './stocks-statistics-list.component.scss',
  standalone: true,
})
export class StocksStatisticsListComponent implements OnInit {
  private store = inject(Store);

  logs$: Observable<StockLogViewModel[]> = this.store.select(StockLogsState.logs);
  groupedLogs$!: Observable<UserScanningStats[]>;

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());

    this.groupedLogs$ = this.logs$.pipe(
      map(logs => {
        const groupedMap = logs.reduce((acc, log) => {
          if (!log.createdBy) {
            return acc;
          }

          if (!acc.has(log.createdBy)) {
            acc.set(log.createdBy, 0);
          }

          acc.set(log.createdBy, acc.get(log.createdBy)! + log.changeQuantity);
          return acc;
        }, new Map<string, number>());

        return Array.from(groupedMap.entries()).map(([createdBy, totalScanned]) => ({
          createdBy,
          totalScanned,
        }));
      })
    );
  }
}
