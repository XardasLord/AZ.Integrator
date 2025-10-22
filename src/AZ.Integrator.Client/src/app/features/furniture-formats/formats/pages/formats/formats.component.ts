import { Component } from '@angular/core';
import { FurnitureDefinitionsListComponent } from '../../components/furniture-definitions-list/furniture-definitions-list.component';
import { FurnitureFormatsFiltersComponent } from '../../components/furniture-formats-filters/furniture-formats-filters.component';
import { FurnitureDefinitionsFixedButtonsComponent } from '../../components/furniture-definitions-fixed-buttons/furniture-definitions-fixed-buttons.component';

@Component({
  selector: 'app-formats',
  imports: [
    FurnitureDefinitionsListComponent,
    FurnitureFormatsFiltersComponent,
    FurnitureDefinitionsFixedButtonsComponent,
  ],
  templateUrl: './formats.component.html',
  styleUrl: './formats.component.scss',
  standalone: true,
})
export class FormatsComponent {}
