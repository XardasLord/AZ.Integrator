import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersComponent } from './pages/orders/orders.component';
import { OrdersListNewComponent } from './pages/orders-list-new/orders-list-new.component';
import { OrdersState } from './states/orders-state.service';
import { OrdersService } from './services/orders.service';
import { RegisterShipmentModalComponent } from './pages/register-shipment-modal/register-shipment-modal.component';
import { OrdersListReadyForShipmentComponent } from './pages/orders-list-ready-for-shipment/orders-list-ready-for-shipment.component';
import { OrdersListSentComponent } from './pages/orders-list-sent/orders-list-sent.component';
import { InvoicesService } from './services/invoices.service';
import { InvoicesState } from './states/invoices.state';
import { OrdersFiltersComponent } from './pages/orders-filters/orders-filters.component';

@NgModule({
  declarations: [
    OrdersComponent,
    OrdersListNewComponent,
    OrdersListReadyForShipmentComponent,
    RegisterShipmentModalComponent,
    OrdersListSentComponent,
    OrdersFiltersComponent,
  ],
  imports: [SharedModule, OrdersRoutingModule, NgxsModule.forFeature([OrdersState, InvoicesState])],
  providers: [OrdersService, InvoicesService],
})
export class OrdersModule {}
