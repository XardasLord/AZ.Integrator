import { Component, inject, OnInit } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StockGroupFormDialogResponseModel } from './stock-group-form-dialog-response.model';
import { StockGroupFormDialogModel } from './stock-group-form-dialog.model';

@Component({
  selector: 'app-stock-group-form-dialog',
  imports: [SharedModule],
  templateUrl: './stock-group-form-dialog.component.html',
  styleUrl: './stock-group-form-dialog.component.scss',
  standalone: true,
})
export class StockGroupFormDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<StockGroupFormDialogComponent>);
  private data: StockGroupFormDialogModel = inject(MAT_DIALOG_DATA);

  groupForm!: FormGroup;
  editMode: boolean = false;

  ngOnInit(): void {
    this.groupForm = this.fb.group({
      name: [this.data?.name || '', Validators.required],
      description: [this.data?.description || ''],
    });

    this.editMode = !!this.data?.name;
  }

  onSubmit() {
    if (this.groupForm.valid) {
      this.dialogRef.close(this.groupForm.value as StockGroupFormDialogResponseModel);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
