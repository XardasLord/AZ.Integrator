import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { ParcelTemplatesComponent } from './pages/parcel-templates/parcel-templates.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ParcelTemplatesRoutingModule } from './parcel-templates-routing.module';
import { ParcelTemplatesListComponent } from './pages/parcel-templates-list/parcel-templates-list.component';
import { ParcelTemplatesService } from './services/parcel-templates.service';
import { ParcelTemplatesState } from './states/parcel-templates.state';
import { ParcelTemplateDefinitionModalComponent } from './pages/parcel-template-definition-modal/parcel-template-definition-modal.component';
import { ParcelTemplatesFiltersComponent } from './pages/parcel-templates-filters/parcel-templates-filters.component';

@NgModule({
  declarations: [
    ParcelTemplatesComponent,
    ParcelTemplatesListComponent,
    ParcelTemplateDefinitionModalComponent,
    ParcelTemplatesFiltersComponent,
  ],
  imports: [
    SharedModule,
    ParcelTemplatesRoutingModule,
    NgxsFormPluginModule,
    NgxsModule.forFeature([ParcelTemplatesState]),
  ],
  providers: [ParcelTemplatesService],
})
export class ParcelTemplatesModule {}
