import { Component } from '@angular/core';
import { StocksListComponent } from '../stocks-list/stocks-list.component';
import { StocksFiltersComponent } from '../../components/stocks-filters/stocks-filters.component';
import { SharedModule } from '../../../../shared/shared.module';
import { StocksFixedButtonsComponent } from '../../components/stocks-fixed-buttons/stocks-fixed-buttons.component';

@Component({
  selector: 'app-stocks',
  imports: [SharedModule, StocksListComponent, StocksFiltersComponent, StocksFixedButtonsComponent],
  templateUrl: './stocks.component.html',
  styleUrl: './stocks.component.scss',
  standalone: true,
})
export class StocksComponent {}
