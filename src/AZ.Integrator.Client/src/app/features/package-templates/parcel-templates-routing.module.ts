import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParcelTemplatesComponent } from './pages/parcel-templates/parcel-templates.component';

const routes: Routes = [
  {
    path: '',
    component: ParcelTemplatesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ParcelTemplatesRoutingModule {}
