import { Component, OnInit, inject } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { SelectionModel } from '@angular/cdk/collections';
import { Store } from '@ngxs/store';
import { map, Observable, of, take } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { OrdersState } from '../../states/orders-state.service';
import {
  ChangePage,
  GenerateDpdLabel,
  GenerateInpostLabel,
  GenerateInpostLabels,
  LoadReadyForShipment,
  SetCurrentTab,
} from '../../states/orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';
import { DownloadInvoice, GenerateInvoice } from '../../states/invoices.action';
import { InvoicesState } from '../../states/invoices.state';
import { getPaymentTypeForOrder } from '../../helpers/payment-type.helper';

@Component({
    selector: 'app-orders-list-ready-for-shipment',
    templateUrl: './orders-list-ready-for-shipment.component.html',
    styleUrls: ['./orders-list-ready-for-shipment.component.scss'],
    standalone: false
})
export class OrdersListReadyForShipmentComponent implements OnInit {
  private store = inject(Store);

  displayedColumns: string[] = [
    'select',
    'shipmentNumber',
    'invoiceNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<OrderDetailsModel>('buyer'),
    'paymentType',
    'deliveryType',
    nameof<OrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'totalToPay',
    'actions',
  ];
  orders$ = this.store.select(OrdersState.getAllNewOrders);
  shipments$ = this.store.select(OrdersState.getShipments);
  invoices$ = this.store.select(InvoicesState.getInvoices);
  totalItems$ = this.store.select(OrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(OrdersState.getCurrentPage);
  pageSize$ = this.store.select(OrdersState.getPageSize);

  selection = new SelectionModel<OrderDetailsModel>(true, []);

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('ReadyForShipment'));
    this.store.dispatch(new LoadReadyForShipment());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
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

  generateShipmentLabelForSelected() {
    if (this.selection.selected.length < 1) {
      return;
    }

    const shipmentIds = this.selection.selected.map(x => x.id);

    this.store.dispatch(new GenerateInpostLabels(shipmentIds));
  }

  generateInvoice(order: OrderDetailsModel) {
    this.store.dispatch(new GenerateInvoice(order.id));
  }

  downloadInvoice(order: OrderDetailsModel) {
    const invoices = this.store.selectSnapshot(InvoicesState.getInvoices);
    const invoiceData = invoices.filter(x => x.externalOrderNumber === order.id)[0];

    this.store.dispatch(new DownloadInvoice(invoiceData.invoiceId, invoiceData.invoiceNumber!));
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

  getPaymentType(order: OrderDetailsModel) {
    return getPaymentTypeForOrder(order);
  }

  isAllSelected(): Observable<boolean> {
    return this.orders$.pipe(
      map(orders => {
        const numSelected = this.selection.selected.length;
        return numSelected === orders.length;
      })
    );
  }

  masterToggle() {
    this.isAllSelected()
      .pipe(take(1))
      .subscribe(allSelected => {
        if (allSelected) {
          this.selection.clear();
        } else {
          this.orders$.pipe(take(1)).subscribe(orders => {
            orders.forEach(row => this.selection.select(row));
          });
        }
      });
  }

  toggleSelection(row: OrderDetailsModel) {
    this.selection.toggle(row);
  }
}
