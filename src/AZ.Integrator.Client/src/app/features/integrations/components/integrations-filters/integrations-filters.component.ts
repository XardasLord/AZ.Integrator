import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationCategory } from '../../models/integration-type.enum';

@Component({
  selector: 'app-integrations-filters',
  standalone: true,
  templateUrl: './integrations-filters.component.html',
  styleUrls: ['./integrations-filters.component.scss'],
  imports: [CommonModule, MaterialModule, FormsModule],
})
export class IntegrationsFiltersComponent {
  @Output() categoryChange = new EventEmitter<IntegrationCategory | null>();
  @Output() searchChange = new EventEmitter<string>();

  selectedCategory: IntegrationCategory | null = null;
  searchText = '';

  categories = [
    { value: null, label: 'Wszystkie', icon: 'apps' },
    { value: IntegrationCategory.Marketplace, label: 'Marketplace', icon: 'storefront' },
    { value: IntegrationCategory.Courier, label: 'Kurierzy', icon: 'local_shipping' },
    { value: IntegrationCategory.Accounting, label: 'Księgowość', icon: 'receipt_long' },
  ];

  selectCategory(category: IntegrationCategory | null): void {
    this.selectedCategory = category;
    this.categoryChange.emit(category);
  }

  onSearchChange(value: string): void {
    this.searchText = value;
    this.searchChange.emit(value);
  }

  clearSearch(): void {
    this.searchText = '';
    this.searchChange.emit('');
  }
}
