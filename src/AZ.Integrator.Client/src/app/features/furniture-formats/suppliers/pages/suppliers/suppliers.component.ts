import { Component } from '@angular/core';
import { SuppliersListComponent } from '../../components/suppliers-list/suppliers-list.component';
import { SuppliersFiltersComponent } from '../../components/suppliers-filters/suppliers-filters.component';
import { SuppliersFixedButtonsComponent } from '../../components/suppliers-fixed-buttons/suppliers-fixed-buttons.component';

@Component({
  selector: 'app-suppliers',
  imports: [SuppliersListComponent, SuppliersFiltersComponent, SuppliersFixedButtonsComponent],
  templateUrl: './suppliers.component.html',
  styleUrl: './suppliers.component.scss',
  standalone: true,
})
export class SuppliersComponent {}
