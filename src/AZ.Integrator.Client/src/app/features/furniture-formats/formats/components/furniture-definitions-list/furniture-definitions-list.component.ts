import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { AsyncPipe } from '@angular/common';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { FormatsState } from '../../states/formats.state';
import { ChangePage, DeleteFurnitureDefinition, LoadFurnitureDefinitions } from '../../states/formats.action';
import { FurnitureDefinitionFormDialogComponent } from '../furniture-definition-form-dialog/furniture-definition-form-dialog.component';
import { ScrollTableComponent } from '../../../../../shared/ui/wrappers/scroll-table/scroll-table.component';
import { FurnitureModelViewModel } from '../../../../../shared/graphql/graphql-integrator.schema';

@Component({
  selector: 'app-furniture-definitions-list',
  templateUrl: './furniture-definitions-list.component.html',
  styleUrls: ['./furniture-definitions-list.component.scss'],
  imports: [MaterialModule, AsyncPipe, ScrollTableComponent],
  standalone: true,
})
export class FurnitureDefinitionsListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = ['furnitureCode', 'partsCount', 'status', 'actions'];
  furnitureDefinitions$ = this.store.select(FormatsState.getFurnitureDefinitions);
  totalItems$ = this.store.select(FormatsState.getTotalCount);
  currentPage$ = this.store.select(FormatsState.getCurrentPage);
  pageSize$ = this.store.select(FormatsState.getPageSize);

  ngOnInit(): void {
    this.store.dispatch(new LoadFurnitureDefinitions());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  editFurnitureDefinition(definition: FurnitureModelViewModel): void {
    const dialogRef = this.dialog.open(FurnitureDefinitionFormDialogComponent, {
      width: '50%',
      maxHeight: '90vh',
      data: definition,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Refresh handled by state
      }
    });
  }

  deleteFurnitureDefinition(furnitureCode: string): void {
    this.store.dispatch(new DeleteFurnitureDefinition(furnitureCode));
  }

  getStatusLabel(isDeleted: boolean): string {
    return isDeleted ? 'Usunięty' : 'Aktywny';
  }

  getStatusClass(isDeleted: boolean): string {
    return isDeleted ? 'text-red-600' : 'text-green-600';
  }
}
