import { Component } from '@angular/core';
import { StocksListComponent } from '../stocks-list/stocks-list.component';
import { StocksFiltersComponent } from '../stocks-filters/stocks-filters.component';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-stocks',
  imports: [SharedModule, StocksListComponent, StocksFiltersComponent],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
  standalone: true,
})
export class StocksComponent {}
