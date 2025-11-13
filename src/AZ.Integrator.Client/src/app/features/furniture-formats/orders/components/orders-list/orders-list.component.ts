import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { AsyncPipe, CommonModule, DatePipe } from '@angular/common';
import { Store } from '@ngxs/store';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { OrdersState } from '../../states/orders.state';
import { ChangePage, LoadOrders } from '../../states/orders.action';
import { ScrollTableComponent } from '../../../../../shared/ui/wrappers/scroll-table/scroll-table.component';
import {
  OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel,
  OrderStatusViewModel,
  PartDefinitionsOrderViewModel,
  SupplierViewModel,
} from '../../../../../shared/graphql/graphql-integrator.schema';
import { SuppliersState } from '../../../suppliers/states/suppliers.state';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss'],
  imports: [MaterialModule, AsyncPipe, ScrollTableComponent, DatePipe, CommonModule],
  standalone: true,
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*', display: 'block' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class OrdersListComponent implements OnInit {
  private store = inject(Store);

  displayedColumns: string[] = ['expand', 'number', 'createdAt', 'status', 'supplier'];
  orders$ = this.store.select(OrdersState.getOrders);
  totalItems$ = this.store.select(OrdersState.getTotalCount);
  currentPage$ = this.store.select(OrdersState.getCurrentPage);
  pageSize$ = this.store.select(OrdersState.getPageSize);

  expandedOrder: PartDefinitionsOrderViewModel | null = null;

  ngOnInit(): void {
    this.store.dispatch(new LoadOrders());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  toggleRow(order: PartDefinitionsOrderViewModel): void {
    this.expandedOrder = this.expandedOrder?.id === order.id ? null : order;
  }

  isExpanded = (index: number, row: PartDefinitionsOrderViewModel): boolean => {
    return this.expandedOrder?.id === row.id;
  };

  getSupplierName(supplierId: number): string {
    const suppliers = this.store.selectSnapshot(SuppliersState.getSuppliers);
    const supplier = suppliers.find((s: SupplierViewModel) => s.id === supplierId);
    return supplier?.name || `ID: ${supplierId}`;
  }

  getStatusDisplay(status: OrderStatusViewModel): string {
    const statusMap: Record<OrderStatusViewModel, string> = {
      [OrderStatusViewModel.Registered]: 'Zarejestrowane',
      [OrderStatusViewModel.Sent]: 'Wysłane do dostawcy',
    };
    return statusMap[status] || status;
  }

  getStatusClass(status: OrderStatusViewModel): string {
    const classMap: Record<OrderStatusViewModel, string> = {
      [OrderStatusViewModel.Registered]: 'status-registered',
      [OrderStatusViewModel.Sent]: 'status-sent',
    };
    return classMap[status] || '';
  }

  getEdgeBandingTypeDisplay(type: OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel): string {
    const displayMap: Record<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel, string> = {
      [OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.None]: 'Brak',
      [OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.One]: 'Jednostronna',
      [OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel.Two]: 'Dwustronna',
    };
    return displayMap[type] || type;
  }
}
