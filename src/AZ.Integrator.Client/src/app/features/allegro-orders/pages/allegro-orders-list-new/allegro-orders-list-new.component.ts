import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { Observable, map, of } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  LoadNew,
  OpenRegisterInPostShipmentModal,
  OpenRegisterDpdShipmentModal,
  SetCurrentTab,
} from '../../states/allegro-orders.action';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';
import { getPaymentTypeForAllegroOrder } from '../../helpers/payment-type.helper';

@Component({
  selector: 'app-allegro-orders-list-new',
  templateUrl: './allegro-orders-list-new.component.html',
  styleUrls: ['./allegro-orders-list-new.component.scss'],
})
export class AllegroOrdersListNewComponent implements OnInit {
  displayedColumns: string[] = [
    nameof<LineItemDetails>('boughtAt'),
    nameof<AllegroOrderDetailsModel>('id'),
    nameof<AllegroOrderDetailsModel>('buyer'),
    'isPaid',
    'paymentType',
    'deliveryType',
    nameof<AllegroOrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'totalToPay',
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getAllNewOrders);
  inpostShipments$ = this.store.select(AllegroOrdersState.getInpostShipments);
  dpdShipments$ = this.store.select(AllegroOrdersState.getInpostShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('New'));
    this.store.dispatch(new LoadNew());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerInPostShipment(order: AllegroOrderDetailsModel) {
    this.store.dispatch(new OpenRegisterInPostShipmentModal(order));
  }

  registerDpdShipment(order: AllegroOrderDetailsModel) {
    this.store.dispatch(new OpenRegisterDpdShipmentModal(order));
  }

  canRegisterInpostShipment(order: AllegroOrderDetailsModel): Observable<boolean> {
    return (
      // of(order.delivery.method.name.toLowerCase().includes('inpost')) ||
      this.inpostShipments$.pipe(
        map(shipments => shipments.every(shipment => shipment.allegroOrderNumber !== order.id))
      )
    );
  }

  canRegisterDpdShipment(order: AllegroOrderDetailsModel): Observable<boolean> {
    return (
      of(order.delivery.method.name.toLowerCase().includes('dpd')) ||
      this.dpdShipments$.pipe(map(shipments => shipments.every(shipment => shipment.allegroOrderNumber !== order.id)))
    );
  }

  getPaymentType(order: AllegroOrderDetailsModel) {
    return getPaymentTypeForAllegroOrder(order);
  }
}
