import { Component } from '@angular/core';
import { OrdersListComponent } from '../../components/orders-list/orders-list.component';
import { OrdersFixedButtonsComponent } from '../../components/orders-fixed-buttons/orders-fixed-buttons.component';
import { OrdersFiltersComponent } from '../../components/orders-filters/orders-filters.component';

@Component({
  selector: 'app-orders',
  imports: [OrdersListComponent, OrdersFiltersComponent, OrdersFixedButtonsComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
  standalone: true,
})
export class OrdersComponent {}
