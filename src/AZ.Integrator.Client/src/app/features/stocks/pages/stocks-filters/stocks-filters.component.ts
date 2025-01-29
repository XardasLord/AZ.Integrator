import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { StocksState } from '../../states/stocks.state';
import { ApplyFilter } from '../../states/stocks.action';

@Component({
  selector: 'app-stocks-filters',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './stocks-filters.component.html',
  styleUrl: './stocks-filters.component.scss',
})
export class StocksFiltersComponent {
  searchText$: Observable<string> = this.store.select(StocksState.getSearchText);

  constructor(private store: Store) {}

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
