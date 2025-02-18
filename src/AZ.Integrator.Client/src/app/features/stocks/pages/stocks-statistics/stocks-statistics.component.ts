import { Component } from '@angular/core';
import { StocksStatisticsListComponent } from '../stocks-statistics-list/stocks-statistics-list.component';
import { StocksStatisticsFiltersComponent } from '../../components/stocks-statistics-filters/stocks-statistics-filters.component';

@Component({
  selector: 'app-stocks-statistics',
  imports: [StocksStatisticsFiltersComponent, StocksStatisticsListComponent],
  templateUrl: './stocks-statistics.component.html',
  styleUrl: './stocks-statistics.component.scss',
  standalone: true,
})
export class StocksStatisticsComponent {}
