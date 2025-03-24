import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { StockLogsState } from '../../states/stock-logs.state';
import { ApplyFilters } from '../../states/stock-logs.action';

@Component({
  selector: 'app-stocks-statistics-filters',
  imports: [SharedModule],
  templateUrl: './stocks-statistics-filters.component.html',
  styleUrl: './stocks-statistics-filters.component.scss',
  standalone: true,
})
export class StocksStatisticsFiltersComponent {
  private store = inject(Store);

  dateRange: { start: Date; end: Date } = {
    start: this.store.selectSnapshot(StockLogsState.dateFilter).from,
    end: this.store.selectSnapshot(StockLogsState.dateFilter).to,
  };

  searchText$: Observable<string> = this.store.select(StockLogsState.searchText);

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilters(searchText, this.dateRange.start!, this.dateRange.end!));
  }

  dateRangeChanged() {
    if (this.dateRange.start && this.dateRange.end) {
      const startDate = new Date(this.dateRange.start);
      const endDate = new Date(this.dateRange.end);

      startDate.setHours(0, 0, 0, 0);
      endDate.setHours(23, 59, 59, 999);

      this.store.dispatch(new ApplyFilters(this.store.selectSnapshot(StockLogsState.searchText), startDate, endDate));
    }
  }
}
