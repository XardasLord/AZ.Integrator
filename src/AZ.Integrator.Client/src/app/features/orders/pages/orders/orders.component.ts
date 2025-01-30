import { Component, ViewChild, inject } from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { Store } from '@ngxs/store';
import { LoadNew, LoadReadyForShipment, LoadSent } from '../../states/orders.action';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.scss'],
    standalone: false
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
