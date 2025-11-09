import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { filter, firstValueFrom, map, Observable, of, tap } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { OrdersState } from '../../states/orders-state.service';
import { ChangePage, GenerateDpdLabel, GenerateInpostLabel, LoadSent, SetCurrentTab } from '../../states/orders.action';
import { ShipmentProviderEnum } from '../../models/shipment-provider.enum';
import { AsyncPipe, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { InvoicesState } from '../../states/invoices.state';
import { DownloadInvoice, GenerateInvoice } from '../../states/invoices.action';
import { MatDialog } from '@angular/material/dialog';
import {
  ConfirmationDialogComponent,
  ConfirmationDialogModel,
} from '../../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SourceSystemState } from '../../../../shared/states/source-system.state';
import { ScrollTableComponent } from '../../../../shared/ui/wrappers/scroll-table/scroll-table.component';

@Component({
  selector: 'app-orders-list-sent',
  templateUrl: './orders-list-sent.component.html',
  styleUrls: ['./orders-list-sent.component.scss'],
  imports: [MaterialModule, NgFor, NgIf, AsyncPipe, DecimalPipe, DatePipe, ScrollTableComponent],
  standalone: true,
})
export class OrdersListSentComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

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
  selectedStore$ = this.store.select(SourceSystemState.getSourceSystem);

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('Sent'));
    const selectedStore = this.store.selectSnapshot(SourceSystemState.getSourceSystem);
    if (selectedStore) {
      this.store.dispatch(new LoadSent());
    }
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

  async generateInvoice(order: OrderDetailsModel) {
    const canDownload = await firstValueFrom(this.canDownloadInvoice(order));

    if (canDownload) {
      this.dialog
        .open(ConfirmationDialogComponent, {
          data: <ConfirmationDialogModel>{
            title: 'Generowanie faktury',
            message: 'Czy na pewno chcesz ponownie wygenerować fakturę dla tego zamówienia?',
          },
        })
        .afterClosed()
        .pipe(
          filter(Boolean),
          takeUntilDestroyed(this.destroyRef),
          tap(() => this.store.dispatch(new GenerateInvoice(order.id)))
        )
        .subscribe();
    } else {
      this.store.dispatch(new GenerateInvoice(order.id));
    }
  }

  downloadInvoice(order: OrderDetailsModel) {
    const invoices = this.store.selectSnapshot(InvoicesState.getInvoices);
    const invoiceData = invoices.filter(x => x.externalOrderNumber === order.id)[0];

    this.store.dispatch(new DownloadInvoice(invoiceData.invoiceId, invoiceData.invoiceNumber!));
  }
}
