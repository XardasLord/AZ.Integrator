import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  GenerateInpostLabel,
  LoadShipments,
  LoadReadyForShipment,
  GenerateDpdLabel,
} from '../../states/allegro-orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';

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
  shipments$ = this.store.select(AllegroOrdersState.getShipments);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch([new LoadReadyForShipment(), new LoadShipments()]);
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  generateShipmentLabel(order: AllegroOrderDetailsModel) {
    const shipment = this.store
      .selectSnapshot(AllegroOrdersState.getShipments)
      .filter(x => x.allegroOrderNumber === order.id)[0];

    if (shipment.shipmentProvider === ShipmentProviderEnum.Inpost) {
      this.store.dispatch(new GenerateInpostLabel(order.id));
    } else if (shipment.shipmentProvider === ShipmentProviderEnum.Dpd) {
      this.store.dispatch(new GenerateDpdLabel(order.id));
    }
  }

  canGenerateShipmentLabel(order: AllegroOrderDetailsModel): Observable<boolean> {
    return this.shipments$.pipe(map(shipments => shipments.some(shipment => shipment.allegroOrderNumber === order.id)));
  }

  getShipmentNumber(order: AllegroOrderDetailsModel): Observable<string | undefined | null> {
    return this.shipments$.pipe(
      map(shipments => shipments.filter(shipment => shipment.allegroOrderNumber === order.id)[0].shipmentNumber)
    );
  }
}
