import { Component, inject, OnInit } from '@angular/core';
import { StocksStatisticsListComponent } from '../stocks-statistics-list/stocks-statistics-list.component';
import { StocksStatisticsFiltersComponent } from '../../components/stocks-statistics-filters/stocks-statistics-filters.component';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { StockLogViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StockLogsState } from '../../states/stock-logs.state';
import { LoadLogs } from '../../states/stock-logs.action';
import { StocksStatisticsChartsComponent } from '../stocks-statistics-charts/stocks-statistics-charts.component';

export interface UserScanningStats {
  createdBy: string;
  totalScanned: number;
  packageStats: UserScanningPackageStats[];
}

export interface UserScanningPackageStats {
  packageCode: string;
  totalScanned: number;
}

@Component({
  selector: 'app-stocks-statistics',
  imports: [StocksStatisticsFiltersComponent, StocksStatisticsListComponent, StocksStatisticsChartsComponent],
  templateUrl: './stocks-statistics.component.html',
  styleUrl: './stocks-statistics.component.scss',
  standalone: true,
})
export class StocksStatisticsComponent implements OnInit {
  private store = inject(Store);

  logs$: Observable<StockLogViewModel[]> = this.store.select(StockLogsState.logs);
  groupedLogs$!: Observable<UserScanningStats[]>;

  ngOnInit(): void {
    this.store.dispatch(new LoadLogs());

    this.groupedLogs$ = this.logs$.pipe(
      map(logs => {
        const groupedMap = new Map<string, { totalScanned: number; packageMap: Map<string, number> }>();

        for (const log of logs) {
          if (!log.createdBy) continue;

          if (!groupedMap.has(log.createdBy)) {
            groupedMap.set(log.createdBy, {
              totalScanned: 0,
              packageMap: new Map<string, number>(),
            });
          }

          const userData = groupedMap.get(log.createdBy)!;
          userData.totalScanned += log.changeQuantity;

          const packageCode = log.packageCode ?? 'UNKNOWN';
          userData.packageMap.set(packageCode, (userData.packageMap.get(packageCode) || 0) + log.changeQuantity);
        }

        return Array.from(groupedMap.entries())
          .map(([createdBy, { totalScanned, packageMap }]) => ({
            createdBy,
            totalScanned,
            packageStats: Array.from(packageMap.entries())
              .map(([packageCode, totalScanned]) => ({ packageCode, totalScanned }))
              .sort((a, b) => b.totalScanned - a.totalScanned), // sortowanie paczek
          }))
          .sort((a, b) => b.totalScanned - a.totalScanned); // sortowanie użytkowników
      })
    );
  }
}
