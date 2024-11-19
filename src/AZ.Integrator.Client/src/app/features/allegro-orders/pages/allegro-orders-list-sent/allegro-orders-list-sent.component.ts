import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  GenerateDpdLabel,
  GenerateInpostLabel,
  LoadSent,
  SetCurrentTab,
} from '../../states/allegro-orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';

@Component({
  selector: 'app-allegro-orders-list-sent',
  templateUrl: './allegro-orders-list-sent.component.html',
  styleUrls: ['./allegro-orders-list-sent.component.scss'],
})
export class AllegroOrdersListSentComponent implements OnInit {
  displayedColumns: string[] = [
    'shipmentNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<OrderDetailsModel>('buyer'),
    'deliveryType',
    nameof<OrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getAllNewOrders);
  shipments$ = this.store.select(AllegroOrdersState.getShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('Sent'));
    this.store.dispatch(new LoadSent());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  getShipmentNumber(order: OrderDetailsModel): Observable<string | undefined | null> {
    return this.shipments$.pipe(
      map(shipments => shipments.filter(shipment => shipment.externalOrderNumber === order.id)[0]?.shipmentNumber)
    );
  }

  canGenerateShipmentLabel(order: OrderDetailsModel): Observable<boolean> {
    return this.shipments$.pipe(
      map(shipments => shipments.some(shipment => shipment.externalOrderNumber === order.id))
    );
  }

  generateShipmentLabel(order: OrderDetailsModel) {
    const shipment = this.store
      .selectSnapshot(AllegroOrdersState.getShipments)
      .filter(x => x.externalOrderNumber === order.id)[0];

    if (shipment.shipmentProvider === ShipmentProviderEnum.Inpost) {
      this.store.dispatch(new GenerateInpostLabel(order.id));
    } else if (shipment.shipmentProvider === ShipmentProviderEnum.Dpd) {
      this.store.dispatch(new GenerateDpdLabel(order.id));
    }
  }
}
