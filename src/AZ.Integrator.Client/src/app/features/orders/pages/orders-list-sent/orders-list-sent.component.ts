import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable, of } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { OrdersState } from '../../states/orders-state.service';
import { ChangePage, GenerateDpdLabel, GenerateInpostLabel, LoadSent, SetCurrentTab } from '../../states/orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';
import { AsyncPipe, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { InvoicesState } from '../../states/invoices.state';
import { DownloadInvoice, GenerateInvoice } from '../../states/invoices.action';

@Component({
  selector: 'app-orders-list-sent',
  templateUrl: './orders-list-sent.component.html',
  styleUrls: ['./orders-list-sent.component.scss'],
  imports: [MaterialModule, NgFor, NgIf, AsyncPipe, DecimalPipe, DatePipe],
})
export class OrdersListSentComponent implements OnInit {
  private store = inject(Store);

  displayedColumns: string[] = [
    'shipmentNumber',
    'invoiceNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<OrderDetailsModel>('buyer'),
    'deliveryType',
    nameof<OrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'actions',
  ];
  orders$ = this.store.select(OrdersState.getAllNewOrders);
  shipments$ = this.store.select(OrdersState.getShipments);
  invoices$ = this.store.select(InvoicesState.getInvoices);
  totalItems$ = this.store.select(OrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(OrdersState.getCurrentPage);
  pageSize$ = this.store.select(OrdersState.getPageSize);

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

  getInvoiceNumber(order: OrderDetailsModel): Observable<string | undefined | null> {
    return this.invoices$.pipe(
      map(invoices => invoices.filter(invoice => invoice.externalOrderNumber === order.id)[0]?.invoiceNumber)
    );
  }

  canGenerateShipmentLabel(order: OrderDetailsModel): Observable<boolean> {
    return this.shipments$.pipe(
      map(shipments => shipments.some(shipment => shipment.externalOrderNumber === order.id))
    );
  }

  canGenerateInvoice(order: OrderDetailsModel): Observable<boolean> {
    return of(true);
  }

  canDownloadInvoice(order: OrderDetailsModel): Observable<boolean> {
    return this.invoices$.pipe(map(invoices => invoices.some(invoice => invoice.externalOrderNumber === order.id)));
  }

  generateShipmentLabel(order: OrderDetailsModel) {
    const shipment = this.store
      .selectSnapshot(OrdersState.getShipments)
      .filter(x => x.externalOrderNumber === order.id)[0];

    if (shipment.shipmentProvider === ShipmentProviderEnum.Inpost) {
      this.store.dispatch(new GenerateInpostLabel(order.id));
    } else if (shipment.shipmentProvider === ShipmentProviderEnum.Dpd) {
      this.store.dispatch(new GenerateDpdLabel(order.id));
    }
  }

  generateInvoice(order: OrderDetailsModel) {
    this.store.dispatch(new GenerateInvoice(order.id));
  }

  downloadInvoice(order: OrderDetailsModel) {
    const invoices = this.store.selectSnapshot(InvoicesState.getInvoices);
    const invoiceData = invoices.filter(x => x.externalOrderNumber === order.id)[0];

    this.store.dispatch(new DownloadInvoice(invoiceData.invoiceId, invoiceData.invoiceNumber!));
  }
}
