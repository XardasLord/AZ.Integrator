import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { PageEvent } from '@angular/material/paginator';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { FurnitureModelViewModel } from '../../../../../shared/graphql/graphql-integrator.schema';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-furniture-selector',
  templateUrl: './furniture-selector.component.html',
  styleUrls: ['./furniture-selector.component.scss'],
  imports: [MaterialModule, FormsModule],
  standalone: true,
})
export class FurnitureSelectorComponent implements OnChanges {
  @Input() furnitureDefinitions: FurnitureModelViewModel[] | null = [];
  @Output() selectionChange = new EventEmitter<FurnitureModelViewModel[]>();

  selectedDefinitions: FurnitureModelViewModel[] = [];
  searchText: string = '';
  filteredDefinitions: FurnitureModelViewModel[] = [];
  pageSize: number = 10;
  currentPage: number = 0;
  pagedDefinitions: FurnitureModelViewModel[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['furnitureDefinitions']) {
      this.filterDefinitions();
    }
  }

  filterDefinitions(): void {
    const definitions = this.furnitureDefinitions || [];

    if (this.searchText.trim()) {
      const searchLower = this.searchText.toLowerCase();
      this.filteredDefinitions = definitions.filter(
        def => def.furnitureCode.toLowerCase().includes(searchLower) || def.version.toString().includes(searchLower)
      );
    } else {
      this.filteredDefinitions = [...definitions];
    }

    this.currentPage = 0;
    this.updatePagedDefinitions();
  }

  updatePagedDefinitions(): void {
    const startIndex = this.currentPage * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedDefinitions = this.filteredDefinitions.slice(startIndex, endIndex);
  }

  onSearchChange(): void {
    this.filterDefinitions();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePagedDefinitions();
  }

  toggleSelection(definition: FurnitureModelViewModel): void {
    const index = this.selectedDefinitions.findIndex(
      d => d.furnitureCode === definition.furnitureCode && d.version === definition.version
    );

    if (index >= 0) {
      this.selectedDefinitions.splice(index, 1);
    } else {
      this.selectedDefinitions.push(definition);
    }

    this.selectionChange.emit([...this.selectedDefinitions]);
  }

  isSelected(definition: FurnitureModelViewModel): boolean {
    return this.selectedDefinitions.some(
      d => d.furnitureCode === definition.furnitureCode && d.version === definition.version
    );
  }

  getDefinitionDisplay(definition: FurnitureModelViewModel): string {
    return `${definition.furnitureCode} (v${definition.version})`;
  }

  getPartCount(definition: FurnitureModelViewModel): number {
    return definition.partDefinitions?.length || 0;
  }
}
