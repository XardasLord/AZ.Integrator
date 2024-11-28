import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { OrdersComponent } from './pages/orders/orders.component';

const routes: Routes = [
  {
    path: '',
    component: OrdersComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), NgxsFormPluginModule],
  exports: [RouterModule],
})
export class OrdersRoutingModule {}
