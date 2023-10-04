import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  GenerateInpostLabel,
  Load,
  LoadInpostShipments,
  OpenRegisterParcelModal,
} from '../../states/allegro-orders.action';
import { AllegroOrderModel, LineItem } from '../../models/allegro-order.model';
import { Observable, map } from 'rxjs';

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
    'items',
    nameof<LineItem>('quantity'),
    nameof<LineItem>('price'),
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getOrders);
  inpostShipments$ = this.store.select(AllegroOrdersState.getInpostShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch([new Load(), new LoadInpostShipments()]);
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerParcel(order: AllegroOrderModel) {
    this.store.dispatch(new OpenRegisterParcelModal(order));
  }

  generateShipmentLabel(order: AllegroOrderModel) {
    this.store.dispatch(new GenerateInpostLabel(order.orderId));
  }

  canRegisterShipment(order: AllegroOrderModel): Observable<boolean> {
    return this.inpostShipments$.pipe(
      map(shipments => shipments.every(shipment => shipment.allegroOrderNumber !== order.orderId))
    );
  }

  canGenerateShipmentLabel(order: AllegroOrderModel): Observable<boolean> {
    return this.inpostShipments$.pipe(
      map(shipments => shipments.some(shipment => shipment.allegroOrderNumber === order.orderId))
    );
  }
}
