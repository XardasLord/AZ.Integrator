import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { PackageTemplatesComponent } from './pages/package-templates/package-templates.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PackageTemplatesRoutingModule } from './package-templates-routing.module';
import { PackageTemplatesListComponent } from './pages/package-templates-list/package-templates-list.component';
import { PackageTemplatesService } from './services/package-templates.service';
import { PackageTemplatesState } from './states/package-templates.state';

@NgModule({
  declarations: [PackageTemplatesComponent, PackageTemplatesListComponent],
  imports: [
    SharedModule,
    PackageTemplatesRoutingModule,
    NgxsFormPluginModule,
    NgxsModule.forFeature([PackageTemplatesState]),
  ],
  providers: [PackageTemplatesService],
})
export class PackageTemplatesModule {}
