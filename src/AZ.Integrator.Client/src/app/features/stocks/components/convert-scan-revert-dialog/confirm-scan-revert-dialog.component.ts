import { Component, Inject } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-scan-revert-dialog',
  imports: [SharedModule],
  templateUrl: './confirm-scan-revert-dialog.component.html',
  styleUrl: './confirm-scan-revert-dialog.component.scss',
  standalone: true,
})
export class ConfirmScanRevertDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { packageCode: string },
    public dialogRef: MatDialogRef<ConfirmScanRevertDialogComponent>
  ) {}
}
