import { Component } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { StocksListComponent } from '../stocks-list/stocks-list.component';
import { StocksFiltersComponent } from '../stocks-filters/stocks-filters.component';

@Component({
    selector: 'app-stocks',
    imports: [SharedModule, StocksListComponent, StocksFiltersComponent],
    templateUrl: './stocks.component.html',
    styleUrl: './stocks.component.scss'
})
export class StocksComponent {}
