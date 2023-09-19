import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { AllegroOrdersRoutingModule } from './allegro-orders-routing.module';
import { AllegroOrdersComponent } from './components/allegro-orders/allegro-orders.component';
import { AllegroOrdersListComponent } from './components/allegro-orders-list/allegro-orders-list.component';
import { AllegroOrdersState } from './states/allegro-orders.state';
import { AllegroOrdersService } from './services/allegro-orders.service';

@NgModule({
  declarations: [AllegroOrdersComponent, AllegroOrdersListComponent],
  imports: [SharedModule, AllegroOrdersRoutingModule, NgxsModule.forFeature([AllegroOrdersState])],
  providers: [AllegroOrdersService],
})
export class AllegroOrdersModule {}
