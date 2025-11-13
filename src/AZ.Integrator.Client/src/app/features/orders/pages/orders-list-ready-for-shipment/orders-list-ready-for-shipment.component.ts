import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { SelectionModel } from '@angular/cdk/collections';
import { Store } from '@ngxs/store';
import { filter, firstValueFrom, map, Observable, of, take, tap } from 'rxjs';
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
import { AsyncPipe, DatePipe, DecimalPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MaterialModule } from '../../../../shared/modules/material.module';
import {
  ConfirmationDialogComponent,
  ConfirmationDialogModel,
} from '../../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { SourceSystemState } from '../../../../shared/states/source-system.state';
import { ScrollTableComponent } from '../../../../shared/ui/wrappers/scroll-table/scroll-table.component';

@Component({
  selector: 'app-orders-list-ready-for-shipment',
  templateUrl: './orders-list-ready-for-shipment.component.html',
  styleUrls: ['./orders-list-ready-for-shipment.component.scss'],
  imports: [MaterialModule, MatIcon, AsyncPipe, DecimalPipe, DatePipe, ScrollTableComponent],
  standalone: true,
})
export class OrdersListReadyForShipmentComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

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
  selectedStore$ = this.store.select(SourceSystemState.getSourceSystem);

  selection = new SelectionModel<OrderDetailsModel>(true, []);

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('ReadyForShipment'));
    const selectedStore = this.store.selectSnapshot(SourceSystemState.getSourceSystem);
    if (selectedStore) {
      this.store.dispatch(new LoadReadyForShipment());
    }
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
      .pipe(take(1), takeUntilDestroyed(this.destroyRef))
      .subscribe(allSelected => {
        if (allSelected) {
          this.selection.clear();
        } else {
          this.orders$.pipe(take(1), takeUntilDestroyed(this.destroyRef)).subscribe(orders => {
            orders.forEach(row => this.selection.select(row));
          });
        }
      });
  }

  toggleSelection(row: OrderDetailsModel) {
    this.selection.toggle(row);
  }
}
