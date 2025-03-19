import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { StocksState } from '../../states/stocks.state';
import { ApplyFilter } from '../../states/stocks.action';

@Component({
  selector: 'app-stocks-filters',
  imports: [SharedModule],
  templateUrl: './stocks-filters.component.html',
  styleUrl: './stocks-filters.component.scss',
  standalone: true,
})
export class StocksFiltersComponent {
  private store = inject(Store);

  searchText$: Observable<string> = this.store.select(StocksState.searchText);

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
