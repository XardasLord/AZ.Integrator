import { Component, inject, ViewChild } from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { Store } from '@ngxs/store';
import { LoadNew, LoadReadyForShipment, LoadSent } from '../../states/orders.action';
import { OrdersFiltersComponent } from '../orders-filters/orders-filters.component';
import { OrdersListNewComponent } from '../orders-list-new/orders-list-new.component';
import { OrdersListReadyForShipmentComponent } from '../orders-list-ready-for-shipment/orders-list-ready-for-shipment.component';
import { OrdersListSentComponent } from '../orders-list-sent/orders-list-sent.component';
import { MaterialModule } from '../../../../shared/modules/material.module';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
  imports: [
    MaterialModule,
    OrdersFiltersComponent,
    OrdersListNewComponent,
    OrdersListReadyForShipmentComponent,
    OrdersListSentComponent,
  ],
})
export class OrdersComponent {
  private store = inject(Store);

  @ViewChild('tabGroup') tabGroup!: MatTabGroup;

  refreshList(): void {
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.store.dispatch(new LoadNew());
        break;
      case 1:
        this.store.dispatch(new LoadReadyForShipment());
        break;
      case 2:
        this.store.dispatch(new LoadSent());
        break;
    }
  }
}
