import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplyFilter } from '../../states/orders.action';
import { OrdersState } from '../../states/orders-state.service';

@Component({
  selector: 'app-orders-filters',
  templateUrl: './orders-filters.component.html',
  styleUrls: ['./orders-filters.component.scss'],
})
export class OrdersFiltersComponent {
  searchText$: Observable<string> = this.store.select(OrdersState.getSearchText);

  constructor(private store: Store) {}

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
