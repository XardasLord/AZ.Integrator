import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { AllegroOrdersRoutingModule } from './allegro-orders-routing.module';
import { AllegroOrdersComponent } from './pages/allegro-orders/allegro-orders.component';
import { AllegroOrdersListComponent } from './pages/allegro-orders-list/allegro-orders-list.component';
import { AllegroOrdersState } from './states/allegro-orders.state';
import { AllegroOrdersService } from './services/allegro-orders.service';
import { RegisterParcelModalComponent } from './pages/register-parcel-modal/register-parcel-modal.component';

@NgModule({
  declarations: [AllegroOrdersComponent, AllegroOrdersListComponent, RegisterParcelModalComponent],
  imports: [SharedModule, AllegroOrdersRoutingModule, NgxsModule.forFeature([AllegroOrdersState])],
  providers: [AllegroOrdersService],
})
export class AllegroOrdersModule {}
