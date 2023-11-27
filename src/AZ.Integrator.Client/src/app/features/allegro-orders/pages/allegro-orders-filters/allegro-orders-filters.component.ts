import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplyFilter } from '../../states/allegro-orders.action';
import { AllegroOrdersState } from '../../states/allegro-orders.state';

@Component({
  selector: 'app-allegro-orders-filters',
  templateUrl: './allegro-orders-filters.component.html',
  styleUrls: ['./allegro-orders-filters.component.scss'],
})
export class AllegroOrdersFiltersComponent {
  searchText$: Observable<string> = this.store.select(AllegroOrdersState.getSearchText);

  constructor(private store: Store) {}

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
