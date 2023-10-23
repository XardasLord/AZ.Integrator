import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { Observable, map } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  LoadNew,
  LoadShipments,
  OpenRegisterInPostShipmentModal,
  OpenRegisterDpdShipmentModal,
} from '../../states/allegro-orders.action';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';

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
    'paymentType',
    'deliveryType',
    nameof<AllegroOrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
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
    this.store.dispatch([new LoadNew(), new LoadShipments()]);
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
    return this.inpostShipments$.pipe(
      map(shipments => shipments.every(shipment => shipment.allegroOrderNumber !== order.id))
    );
  }

  canRegisterDpdShipment(order: AllegroOrderDetailsModel): Observable<boolean> {
    return this.dpdShipments$.pipe(
      map(shipments => shipments.every(shipment => shipment.allegroOrderNumber !== order.id))
    );
  }
}
