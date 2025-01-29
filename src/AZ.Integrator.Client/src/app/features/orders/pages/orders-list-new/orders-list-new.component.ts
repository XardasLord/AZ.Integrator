import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable, of } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { OrdersState } from '../../states/orders-state.service';
import {
  ChangePage,
  LoadNew,
  OpenRegisterDpdShipmentModal,
  OpenRegisterInPostShipmentModal,
  SetCurrentTab,
} from '../../states/orders.action';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { getPaymentTypeForOrder } from '../../helpers/payment-type.helper';

@Component({
    selector: 'app-orders-list-new',
    templateUrl: './orders-list-new.component.html',
    styleUrls: ['./orders-list-new.component.scss'],
    standalone: false
})
export class OrdersListNewComponent implements OnInit {
  displayedColumns: string[] = [
    nameof<LineItemDetails>('boughtAt'),
    nameof<OrderDetailsModel>('buyer'),
    'isPaid',
    'paymentType',
    'deliveryType',
    nameof<OrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'totalToPay',
    'actions',
  ];
  orders$ = this.store.select(OrdersState.getAllNewOrders);
  inpostShipments$ = this.store.select(OrdersState.getInpostShipments);
  dpdShipments$ = this.store.select(OrdersState.getInpostShipments);
  totalItems$ = this.store.select(OrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(OrdersState.getCurrentPage);
  pageSize$ = this.store.select(OrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('New'));
    this.store.dispatch(new LoadNew());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerInPostShipment(order: OrderDetailsModel) {
    this.store.dispatch(new OpenRegisterInPostShipmentModal(order));
  }

  registerDpdShipment(order: OrderDetailsModel) {
    this.store.dispatch(new OpenRegisterDpdShipmentModal(order));
  }

  canRegisterInpostShipment(order: OrderDetailsModel): Observable<boolean> {
    return (
      // of(order.delivery.method.name.toLowerCase().includes('inpost')) ||
      this.inpostShipments$.pipe(
        map(shipments => shipments.every(shipment => shipment.externalOrderNumber !== order.id))
      )
    );
  }

  canRegisterDpdShipment(order: OrderDetailsModel): Observable<boolean> {
    return (
      of(order.delivery.method.name.toLowerCase().includes('dpd')) ||
      this.dpdShipments$.pipe(map(shipments => shipments.every(shipment => shipment.externalOrderNumber !== order.id)))
    );
  }

  getPaymentType(order: OrderDetailsModel) {
    return getPaymentTypeForOrder(order);
  }
}
