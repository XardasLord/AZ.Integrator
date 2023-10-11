import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { filter, map, Observable } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  GenerateInpostLabel,
  LoadInpostShipments,
  LoadReadyForShipment,
  OpenRegisterInPostShipmentModal,
} from '../../states/allegro-orders.action';

@Component({
  selector: 'app-allegro-orders-list-ready-for-shipment',
  templateUrl: './allegro-orders-list-ready-for-shipment.component.html',
  styleUrls: ['./allegro-orders-list-ready-for-shipment.component.scss'],
})
export class AllegroOrdersListReadyForShipmentComponent implements OnInit {
  displayedColumns: string[] = [
    'shipmentNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<AllegroOrderDetailsModel>('id'),
    nameof<AllegroOrderDetailsModel>('buyer'),
    nameof<AllegroOrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getAllNewOrders);
  inpostShipments$ = this.store.select(AllegroOrdersState.getInpostShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch([new LoadReadyForShipment(), new LoadInpostShipments()]);
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerInPostShipment(order: AllegroOrderDetailsModel) {
    this.store.dispatch(new OpenRegisterInPostShipmentModal(order));
  }

  generateShipmentLabel(order: AllegroOrderDetailsModel) {
    this.store.dispatch(new GenerateInpostLabel(order.id));
  }

  canGenerateShipmentLabel(order: AllegroOrderDetailsModel): Observable<boolean> {
    return this.inpostShipments$.pipe(
      map(shipments => shipments.some(shipment => shipment.allegroOrderNumber === order.id))
    );
  }

  getShipmentNumber(order: AllegroOrderDetailsModel): Observable<string | undefined | null> {
    return this.inpostShipments$.pipe(
      map(shipments => shipments.filter(shipment => shipment.allegroOrderNumber === order.id)[0].inpostShipmentNumber)
    );
  }
}
