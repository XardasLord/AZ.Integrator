import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StockThresholdFormDialogResponseModel } from './stock-threshold-form-dialog-response.model';
import { StockThresholdFormDialogModel } from './stock-threshold-form-dialog.model';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-stock-group-form-dialog',
  imports: [SharedModule],
  templateUrl: './stock-threshold-form-dialog.component.html',
  styleUrl: './stock-threshold-form-dialog.component.scss',
  standalone: true,
})
export class StockThresholdFormDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<StockThresholdFormDialogComponent>);
  data: StockThresholdFormDialogModel = inject(MAT_DIALOG_DATA);

  groupForm!: FormGroup;

  ngOnInit(): void {
    this.groupForm = this.fb.group({
      threshold: [this.data?.threshold || 0, [Validators.required, Validators.min(1)]],
    });
  }

  save() {
    if (this.groupForm.valid) {
      this.dialogRef.close(this.groupForm.value as StockThresholdFormDialogResponseModel);
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}
