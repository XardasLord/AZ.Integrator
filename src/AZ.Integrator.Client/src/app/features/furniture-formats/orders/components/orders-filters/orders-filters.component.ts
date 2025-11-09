import { Component, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngxs/store';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { OrdersState } from '../../states/orders.state';
import { ApplyFilter } from '../../states/orders.action';

@Component({
  selector: 'app-orders-filters',
  templateUrl: './orders-filters.component.html',
  styleUrls: ['./orders-filters.component.scss'],
  imports: [MaterialModule, ReactiveFormsModule, NgIf],
  standalone: true,
})
export class OrdersFiltersComponent {
  private store = inject(Store);

  searchControl = new FormControl<string>(this.store.selectSnapshot(OrdersState.getSearchText));

  constructor() {
    this.searchControl.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe(value => {
      this.store.dispatch(new ApplyFilter(value || ''));
    });
  }
}
