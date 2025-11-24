import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { AddIntegrationDialogComponent } from '../add-integration-dialog/add-integration-dialog.component';

@Component({
  selector: 'app-add-integration-button',
  standalone: true,
  templateUrl: './add-integration-button.component.html',
  styleUrls: ['./add-integration-button.component.scss'],
  imports: [CommonModule, MaterialModule],
})
export class AddIntegrationButtonComponent {
  private dialog = inject(MatDialog);

  openAddDialog(): void {
    this.dialog.open(AddIntegrationDialogComponent, {
      width: '600px',
      maxHeight: '90vh',
    });
  }
}
