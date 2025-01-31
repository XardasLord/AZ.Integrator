import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplyFilter } from '../../states/orders.action';
import { OrdersState } from '../../states/orders-state.service';
import { DebounceDirective } from '../../../../shared/directives/debounce.directive';
import { AsyncPipe } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';

@Component({
  selector: 'app-orders-filters',
  templateUrl: './orders-filters.component.html',
  styleUrls: ['./orders-filters.component.scss'],
  imports: [MaterialModule, DebounceDirective, AsyncPipe],
})
export class OrdersFiltersComponent {
  private store = inject(Store);

  searchText$: Observable<string> = this.store.select(OrdersState.getSearchText);

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
