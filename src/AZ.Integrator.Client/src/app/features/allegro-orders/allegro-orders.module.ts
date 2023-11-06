import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { AllegroOrdersRoutingModule } from './allegro-orders-routing.module';
import { AllegroOrdersComponent } from './pages/allegro-orders/allegro-orders.component';
import { AllegroOrdersListNewComponent } from './pages/allegro-orders-list-new/allegro-orders-list-new.component';
import { AllegroOrdersState } from './states/allegro-orders.state';
import { AllegroOrdersService } from './services/allegro-orders.service';
import { RegisterShipmentModalComponent } from './pages/register-shipment-modal/register-shipment-modal.component';
import { AllegroOrdersListReadyForShipmentComponent } from './pages/allegro-orders-list-ready-for-shipment/allegro-orders-list-ready-for-shipment.component';
import { AllegroOrdersListSentComponent } from './pages/allegro-orders-list-sent/allegro-orders-list-sent.component';
import { InvoicesService } from './services/invoices.service';
import { InvoicesState } from './states/invoices.state';

@NgModule({
  declarations: [
    AllegroOrdersComponent,
    AllegroOrdersListNewComponent,
    AllegroOrdersListReadyForShipmentComponent,
    RegisterShipmentModalComponent,
    AllegroOrdersListSentComponent,
  ],
  imports: [SharedModule, AllegroOrdersRoutingModule, NgxsModule.forFeature([AllegroOrdersState, InvoicesState])],
  providers: [AllegroOrdersService, InvoicesService],
})
export class AllegroOrdersModule {}
