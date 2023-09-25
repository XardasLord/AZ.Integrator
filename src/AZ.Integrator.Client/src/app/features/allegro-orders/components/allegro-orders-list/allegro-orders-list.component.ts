import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import { ChangePage, Load, RegisterParcel } from '../../states/allegro-orders.action';
import { AllegroOrderModel } from '../../models/allegro-order.model';

@Component({
  selector: 'app-allegro-orders-list',
  templateUrl: './allegro-orders-list.component.html',
  styleUrls: ['./allegro-orders-list.component.scss'],
})
export class AllegroOrdersListComponent implements OnInit {
  displayedColumns: string[] = [
    nameof<AllegroOrderModel>('date'),
    nameof<AllegroOrderModel>('orderId'),
    nameof<AllegroOrderModel>('buyer'),
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getOrders);
  totalItems$ = this.store.select(AllegroOrdersState.getOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new Load());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerParcel(order: AllegroOrderModel) {
    this.store.dispatch(new RegisterParcel(order));
  }
}
