import { NgModule } from '@angular/core';
import { provideStates } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { ParcelTemplatesComponent } from './pages/parcel-templates/parcel-templates.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ParcelTemplatesRoutingModule } from './parcel-templates-routing.module';
import { ParcelTemplatesListComponent } from './pages/parcel-templates-list/parcel-templates-list.component';
import { ParcelTemplatesService } from './services/parcel-templates.service';
import { ParcelTemplatesState } from './states/parcel-templates.state';
import { PackageTemplateDefinitionFormDialogComponent } from './components/package-template-definition-form-dialog/package-template-definition-form-dialog.component';
import { ParcelTemplatesFiltersComponent } from './components/parcel-templates-filters/parcel-templates-filters.component';

@NgModule({
  imports: [
    SharedModule,
    ParcelTemplatesRoutingModule,
    NgxsFormPluginModule,
    ParcelTemplatesComponent,
    ParcelTemplatesListComponent,
    PackageTemplateDefinitionFormDialogComponent,
    ParcelTemplatesFiltersComponent,
  ],
  providers: [provideStates([ParcelTemplatesState]), ParcelTemplatesService],
})
export class ParcelTemplatesModule {}
