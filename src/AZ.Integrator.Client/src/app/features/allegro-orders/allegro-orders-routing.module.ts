import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { AllegroOrdersComponent } from './pages/allegro-orders/allegro-orders.component';

const routes: Routes = [
  {
    path: '',
    component: AllegroOrdersComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), NgxsFormPluginModule],
  exports: [RouterModule],
})
export class AllegroOrdersRoutingModule {}
