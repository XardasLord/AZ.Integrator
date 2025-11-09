import { Component, inject } from '@angular/core';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { Store } from '@ngxs/store';
import { LoadOrders } from '../../states/orders.action';
import { Navigate } from '@ngxs/router-plugin';
import { FurnitureFormatsRoutePath } from '../../../../../core/modules/app-routing.module';

@Component({
  selector: 'app-orders-fixed-buttons',
  templateUrl: './orders-fixed-buttons.component.html',
  styleUrls: ['./orders-fixed-buttons.component.scss'],
  imports: [MaterialModule],
  standalone: true,
})
export class OrdersFixedButtonsComponent {
  private store = inject(Store);

  onCreateOrder(): void {
    this.store.dispatch(new Navigate([`${FurnitureFormatsRoutePath.OrdersCreate}`]));
  }

  refreshList(): void {
    this.store.dispatch(new LoadOrders());
  }
}
