import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { Store } from '@ngxs/store';
import { LoadNew, LoadReadyForShipment, LoadSent } from '../../states/orders.action';
import { OrdersFiltersComponent } from '../orders-filters/orders-filters.component';
import { OrdersListNewComponent } from '../orders-list-new/orders-list-new.component';
import { OrdersListReadyForShipmentComponent } from '../orders-list-ready-for-shipment/orders-list-ready-for-shipment.component';
import { OrdersListSentComponent } from '../orders-list-sent/orders-list-sent.component';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { LoadIntegrations } from '../../../integrations/states/integrations.action';

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
export class OrdersComponent implements OnInit {
  private store = inject(Store);

  @ViewChild('tabGroup') tabGroup!: MatTabGroup;

  ngOnInit() {
    this.store.dispatch(new LoadIntegrations());
  }

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
