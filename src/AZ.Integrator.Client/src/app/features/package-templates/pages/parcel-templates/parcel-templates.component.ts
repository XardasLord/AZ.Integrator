import { Component } from '@angular/core';
import { ParcelTemplatesFiltersComponent } from '../../components/parcel-templates-filters/parcel-templates-filters.component';
import { ParcelTemplatesListComponent } from '../parcel-templates-list/parcel-templates-list.component';
import { PackageTemplatesFixedButtonsComponent } from '../../components/package-templates-fixed-buttons/package-templates-fixed-buttons.component';

@Component({
  selector: 'app-parcel-templates',
  templateUrl: './parcel-templates.component.html',
  styleUrls: ['./parcel-templates.component.scss'],
  imports: [ParcelTemplatesFiltersComponent, ParcelTemplatesListComponent, PackageTemplatesFixedButtonsComponent],
})
export class ParcelTemplatesComponent {}
