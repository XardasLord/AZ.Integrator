import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import { ChangePage, LoadShipments, LoadSent } from '../../states/allegro-orders.action';

@Component({
  selector: 'app-allegro-orders-list-sent',
  templateUrl: './allegro-orders-list-sent.component.html',
  styleUrls: ['./allegro-orders-list-sent.component.scss'],
})
export class AllegroOrdersListSentComponent implements OnInit {
  displayedColumns: string[] = [
    'shipmentNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<AllegroOrderDetailsModel>('id'),
    nameof<AllegroOrderDetailsModel>('buyer'),
    'deliveryType',
    nameof<AllegroOrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
  ];
  orders$ = this.store.select(AllegroOrdersState.getAllNewOrders);
  inpostShipments$ = this.store.select(AllegroOrdersState.getInpostShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch([new LoadSent(), new LoadShipments()]);
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  getShipmentNumber(order: AllegroOrderDetailsModel): Observable<string | undefined | null> {
    return this.inpostShipments$.pipe(
      map(shipments => shipments.filter(shipment => shipment.allegroOrderNumber === order.id)[0].shipmentNumber)
    );
  }
}
