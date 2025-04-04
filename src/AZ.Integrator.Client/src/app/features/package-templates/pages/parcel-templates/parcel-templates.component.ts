import { Component } from '@angular/core';
import { ParcelTemplatesFiltersComponent } from '../parcel-templates-filters/parcel-templates-filters.component';
import { ParcelTemplatesListComponent } from '../parcel-templates-list/parcel-templates-list.component';

@Component({
    selector: 'app-parcel-templates',
    templateUrl: './parcel-templates.component.html',
    styleUrls: ['./parcel-templates.component.scss'],
    imports: [ParcelTemplatesFiltersComponent, ParcelTemplatesListComponent]
})
export class ParcelTemplatesComponent {}
