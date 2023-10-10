import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { AllegroOrdersRoutingModule } from './allegro-orders-routing.module';
import { AllegroOrdersComponent } from './pages/allegro-orders/allegro-orders.component';
import { AllegroOrdersListNewComponent } from './pages/allegro-orders-list-new/allegro-orders-list-new.component';
import { AllegroOrdersState } from './states/allegro-orders.state';
import { AllegroOrdersService } from './services/allegro-orders.service';
import { RegisterParcelModalComponent } from './pages/register-parcel-modal/register-parcel-modal.component';
import { AllegroOrdersListReadyForShipmentComponent } from './pages/allegro-orders-list-ready-for-shipment/allegro-orders-list-ready-for-shipment.component';

@NgModule({
  declarations: [
    AllegroOrdersComponent,
    AllegroOrdersListNewComponent,
    AllegroOrdersListReadyForShipmentComponent,
    RegisterParcelModalComponent,
  ],
  imports: [SharedModule, AllegroOrdersRoutingModule, NgxsModule.forFeature([AllegroOrdersState])],
  providers: [AllegroOrdersService],
})
export class AllegroOrdersModule {}
