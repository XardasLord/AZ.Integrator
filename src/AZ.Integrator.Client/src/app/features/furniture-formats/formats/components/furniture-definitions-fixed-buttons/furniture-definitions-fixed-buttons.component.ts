import { Component, DestroyRef, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { Store } from '@ngxs/store';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FurnitureDefinitionFormDialogComponent } from '../furniture-definition-form-dialog/furniture-definition-form-dialog.component';
import { LoadFurnitureDefinitions } from '../../states/formats.action';
import { MatIcon } from '@angular/material/icon';
import { MatFabButton } from '@angular/material/button';
import { MatTooltip } from '@angular/material/tooltip';

@Component({
  selector: 'app-furniture-definitions-fixed-buttons',
  imports: [MatIcon, MatFabButton, MatTooltip],
  templateUrl: './furniture-definitions-fixed-buttons.component.html',
  styleUrl: './furniture-definitions-fixed-buttons.component.scss',
  standalone: true,
})
export class FurnitureDefinitionsFixedButtonsComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  openAddDialog(): void {
    const dialogRef = this.dialog.open(FurnitureDefinitionFormDialogComponent, {
      width: '50%',
      maxHeight: '90vh',
      data: null,
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(result => {
        if (result) {
          // Refresh handled by state
        }
      });
  }

  refreshList() {
    this.store.dispatch(new LoadFurnitureDefinitions());
  }
}
