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

@NgModule({
  declarations: [ParcelTemplatesComponent, ParcelTemplatesListComponent, ParcelTemplateDefinitionModalComponent],
  imports: [
    SharedModule,
    ParcelTemplatesRoutingModule,
    NgxsFormPluginModule,
    NgxsModule.forFeature([ParcelTemplatesState]),
  ],
  providers: [ParcelTemplatesService],
})
export class ParcelTemplatesModule {}
