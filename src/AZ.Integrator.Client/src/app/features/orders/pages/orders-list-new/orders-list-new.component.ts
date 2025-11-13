import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { AsyncPipe, DatePipe, DecimalPipe } from '@angular/common';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatIcon } from '@angular/material/icon';
import { Store } from '@ngxs/store';
import { map, Observable, of } from 'rxjs';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { OrdersState } from '../../states/orders-state.service';
import { ChangePage, LoadNew, OpenRegisterDpdShipmentModal, SetCurrentTab } from '../../states/orders.action';
import { LineItemDetails, OrderDetailsModel } from '../../models/order-details.model';
import { getPaymentTypeForOrder } from '../../helpers/payment-type.helper';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { SourceSystemState } from '../../../../shared/states/source-system.state';
import { ScrollTableComponent } from '../../../../shared/ui/wrappers/scroll-table/scroll-table.component';
import { RegisterShipmentDataModel } from '../../models/register-shipment-data.model';
import { RegisterShipmentModalComponent } from '../register-shipment-modal/register-shipment-modal.component';

@Component({
  selector: 'app-orders-list-new',
  templateUrl: './orders-list-new.component.html',
  styleUrls: ['./orders-list-new.component.scss'],
  imports: [MaterialModule, MatIcon, AsyncPipe, DecimalPipe, DatePipe, ScrollTableComponent],
  standalone: true,
})
export class OrdersListNewComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private dialogRef!: MatDialogRef<RegisterShipmentModalComponent, RegisterShipmentDataModel>;

  displayedColumns: string[] = [
    nameof<LineItemDetails>('boughtAt'),
    nameof<OrderDetailsModel>('buyer'),
    'isPaid',
    'paymentType',
    'deliveryType',
    nameof<OrderDetailsModel>('lineItems'),
    nameof<LineItemDetails>('quantity'),
    nameof<LineItemDetails>('price'),
    'totalToPay',
    'actions',
  ];
  orders$ = this.store.select(OrdersState.getAllNewOrders);
  inpostShipments$ = this.store.select(OrdersState.getInpostShipments);
  dpdShipments$ = this.store.select(OrdersState.getInpostShipments);
  totalItems$ = this.store.select(OrdersState.getAllNewOrdersCount);
  currentPage$ = this.store.select(OrdersState.getCurrentPage);
  pageSize$ = this.store.select(OrdersState.getPageSize);
  selectedStore$ = this.store.select(SourceSystemState.getSourceSystem);

  ngOnInit(): void {
    this.store.dispatch(new SetCurrentTab('New'));
    const selectedStore = this.store.selectSnapshot(SourceSystemState.getSourceSystem);
    if (selectedStore) {
      this.store.dispatch(new LoadNew());
    }
  }

  ngOnDestroy() {
    console.log('OrdersListNewComponent destroyed');
    this.dialog.closeAll();
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  registerInPostShipment(order: OrderDetailsModel) {
    const data: RegisterShipmentDataModel = {
      order: order,
      deliveryMethodType: 'INPOST',
    };

    this.dialogRef = this.dialog.open<RegisterShipmentModalComponent, RegisterShipmentDataModel>(
      RegisterShipmentModalComponent,
      {
        data: <RegisterShipmentDataModel>data,
        width: '60%',
        height: '82%',
      }
    );
  }

  registerDpdShipment(order: OrderDetailsModel) {
    this.store.dispatch(new OpenRegisterDpdShipmentModal(order));
  }

  canRegisterInpostShipment(order: OrderDetailsModel): Observable<boolean> {
    return (
      // of(order.delivery.method.name.toLowerCase().includes('inpost')) ||
      this.inpostShipments$.pipe(
        map(shipments => shipments.every(shipment => shipment.externalOrderNumber !== order.id))
      )
    );
  }

  canRegisterDpdShipment(order: OrderDetailsModel): Observable<boolean> {
    return (
      of(order.delivery.method.name.toLowerCase().includes('dpd')) ||
      this.dpdShipments$.pipe(map(shipments => shipments.every(shipment => shipment.externalOrderNumber !== order.id)))
    );
  }

  getPaymentType(order: OrderDetailsModel) {
    return getPaymentTypeForOrder(order);
  }
}
