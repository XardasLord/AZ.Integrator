import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ParcelTemplateDefinitionDataModel } from './parcel-template-definition-data.model';
import { TemplatePackageFormGroupModel } from './template-package-form-group.model';
import { ParcelFromGroupModel } from '../../../../shared/models/parcel-form-group.model';
import { ParcelTemplate } from '../../models/parcel-template';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { PackageTemplateDefinitionFormDialogResponseModel } from './package-template-definition-form-dialog-response-model';

@Component({
  selector: 'app-package-template-definition-form-dialog',
  templateUrl: './package-template-definition-form-dialog.component.html',
  styleUrls: ['./package-template-definition-form-dialog.component.scss'],
  imports: [MaterialModule, FormsModule, ReactiveFormsModule, CommonModule],
  standalone: true,
})
export class PackageTemplateDefinitionFormDialogComponent implements OnInit {
  private data: ParcelTemplateDefinitionDataModel = inject(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private dialogRef: MatDialogRef<PackageTemplateDefinitionFormDialogComponent> = inject(MatDialogRef);

  form!: FormGroup<TemplatePackageFormGroupModel>;
  editMode: boolean = false;

  get parcels(): FormArray<FormGroup> {
    return this.form.controls.parcels;
  }

  ngOnInit(): void {
    this.editMode = !!this.data?.tag;

    this.form = this.fb.group<TemplatePackageFormGroupModel>({
      tag: new FormControl<string>(
        {
          value: this.editMode ? this.data?.tag : '',
          disabled: this.editMode,
        },
        Validators.required
      ),
      parcels: this.fb.array<FormGroup>([], [Validators.required]),
    });

    if (this.data?.template?.parcels && this.data?.template?.parcels?.length > 0) {
      this.data.template?.parcels?.forEach(parcel => {
        this.addNewParcel(parcel?.length, parcel?.width, parcel?.height, parcel?.weight);
      });
    } else {
      this.addNewParcel();
    }

    this.form.markAllAsTouched();
  }

  addNewParcel(length = 0, width = 0, height = 0, weight = 0) {
    const parcel = this.fb.group<ParcelFromGroupModel>({
      length: new FormControl<number>(length, [Validators.required, Validators.min(1)]),
      width: new FormControl<number>(width, [Validators.required, Validators.min(1)]),
      height: new FormControl<number>(height, [Validators.required, Validators.min(1)]),
      weight: new FormControl<number>(weight, [Validators.required, Validators.min(1)]),
    });

    this.parcels.push(parcel);
  }

  removeParcel(index: number) {
    this.parcels.removeAt(index);
  }

  onSubmit() {
    if (!this.form.valid) {
      return;
    }

    const parcels: ParcelTemplate[] = [];

    for (let i = 0; i < this.form.value.parcels.length; i++) {
      const parcel = this.form.value.parcels[i];

      parcels.push({
        id: `${i + 1}`,
        weight: parcel.weight.toString(),
        dimensions: {
          length: parcel.length.toString(),
          width: parcel.width.toString(),
          height: parcel.height.toString(),
        },
      });
    }

    const response: PackageTemplateDefinitionFormDialogResponseModel = {
      tag: this.editMode ? this.data.tag : this.form.value.tag!,
      parcels,
    };

    this.dialogRef.close(response);
  }

  onCancel() {
    this.dialogRef.close();
  }
}
