import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { SupplierFormDialogComponent } from '../supplier-form-dialog/supplier-form-dialog.component';
import { LoadSuppliers } from '../../states/suppliers.action';
import { MatIcon } from '@angular/material/icon';
import { MatFabButton } from '@angular/material/button';
import { MatTooltip } from '@angular/material/tooltip';

@Component({
  selector: 'app-suppliers-fixed-buttons',
  imports: [MatIcon, MatFabButton, MatTooltip],
  templateUrl: './suppliers-fixed-buttons.component.html',
  styleUrl: './suppliers-fixed-buttons.component.scss',
  standalone: true,
})
export class SuppliersFixedButtonsComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  openAddDialog(): void {
    const dialogRef = this.dialog.open(SupplierFormDialogComponent, {
      width: '50%',
      maxHeight: '90vh',
      data: null,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Refresh handled by state
      }
    });
  }

  refreshList() {
    this.store.dispatch(new LoadSuppliers());
  }
}
