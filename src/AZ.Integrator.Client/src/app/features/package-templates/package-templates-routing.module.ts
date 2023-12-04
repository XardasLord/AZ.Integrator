import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PackageTemplatesComponent } from './pages/package-templates/package-templates.component';

const routes: Routes = [
  {
    path: '',
    component: PackageTemplatesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PackageTemplatesRoutingModule {}
