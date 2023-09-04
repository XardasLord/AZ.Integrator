import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { TestComponent } from './components/test/test.component';

const routes: Routes = [
  {
    path: '',
    component: TestComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), NgxsFormPluginModule],
  exports: [RouterModule],
})
export class TestRoutingModule {}
