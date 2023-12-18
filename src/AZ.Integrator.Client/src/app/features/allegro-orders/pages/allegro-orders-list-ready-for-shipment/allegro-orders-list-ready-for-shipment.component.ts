import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { map, Observable, of, take } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { AllegroOrderDetailsModel, LineItemDetails } from '../../models/allegro-order-details.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import {
  ChangePage,
  GenerateDpdLabel,
  GenerateInpostLabel,
  GenerateInpostLabels,
  LoadReadyForShipment,
  SetCurrentTab,
} from '../../states/allegro-orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';
import { DownloadInvoice, GenerateInvoice } from '../../states/invoices.action';
import { InvoicesState } from '../../states/invoices.state';
import { getPaymentTypeForAllegroOrder } from '../../helpers/payment-type.helper';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-allegro-orders-list-ready-for-shipment',
  templateUrl: './allegro-orders-list-ready-for-shipment.component.html',
  styleUrls: ['./allegro-orders-list-ready-for-shipment.component.scss'],
})
export class AllegroOrdersListReadyForShipmentComponent implements OnInit {
  displayedColumns: string[] = [
    'select',
    'shipmentNumber',
    'invoiceNumber',
    nameof<LineItemDetails>('boughtAt'),
    nameof<AllegroOrderDetailsModel>('buyer'),
    'paymentType',
    'deliveryType',
    nameof<AllegroOrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'totalToPay',
    'actions',
  ];
  orders$ = this.store.select(AllegroOrdersState.getAllNewOrders);
  shipments$ = this.store.select(AllegroOrdersState.getShipments);
  invoices$ = this.store.select(InvoicesState.getInvoices);
  totalItems$ = this.store.select(AllegroOrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  selection = new SelectionModel<AllegroOrderDetailsModel>(true, []);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('ReadyForShipment'));
    this.store.dispatch(new LoadReadyForShipment());
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

  generateShipmentLabelForSelected() {
    if (this.selection.selected.length < 1) {
      return;
    }

    const shipmentIds = this.selection.selected.map(x => x.id);

    this.store.dispatch(new GenerateInpostLabels(shipmentIds));
  }

  generateInvoice(order: AllegroOrderDetailsModel) {
    this.store.dispatch(new GenerateInvoice(order.id));
  }

  downloadInvoice(order: AllegroOrderDetailsModel) {
    const invoices = this.store.selectSnapshot(InvoicesState.getInvoices);
    const invoiceData = invoices.filter(x => x.allegroOrderNumber === order.id)[0];

    this.store.dispatch(new DownloadInvoice(invoiceData.invoiceId, invoiceData.invoiceNumber!));
    console.warn(this.selection);
  }

  canGenerateShipmentLabel(order: AllegroOrderDetailsModel): Observable<boolean> {
    return this.shipments$.pipe(map(shipments => shipments.some(shipment => shipment.allegroOrderNumber === order.id)));
  }

  canGenerateInvoice(order: AllegroOrderDetailsModel): Observable<boolean> {
    return of(true);
  }

  canDownloadInvoice(order: AllegroOrderDetailsModel): Observable<boolean> {
    return this.invoices$.pipe(map(invoices => invoices.some(invoice => invoice.allegroOrderNumber === order.id)));
  }

  getShipmentNumber(order: AllegroOrderDetailsModel): Observable<string | undefined | null> {
    return this.shipments$.pipe(
      map(shipments => shipments.filter(shipment => shipment.allegroOrderNumber === order.id)[0]?.shipmentNumber)
    );
  }

  getInvoiceNumber(order: AllegroOrderDetailsModel): Observable<string | undefined | null> {
    return this.invoices$.pipe(
      map(invoices => invoices.filter(invoice => invoice.allegroOrderNumber === order.id)[0]?.invoiceNumber)
    );
  }

  getPaymentType(order: AllegroOrderDetailsModel) {
    return getPaymentTypeForAllegroOrder(order);
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

  toggleSelection(row: AllegroOrderDetailsModel) {
    this.selection.toggle(row);
  }
}
