import { Component, OnDestroy, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Subscription } from 'rxjs';
import { ParcelTemplateDefinitionDataModel } from '../../models/parcel-template-definition-data.model';
import { TemplateParcelFormGroupModel } from '../../models/template-parcel-form-group.model';
import { ParcelFromGroupModel } from '../../../../shared/models/parcel-form-group.model';
import { ParcelTemplate } from '../../models/parcel-template';
import { SaveParcelTemplateCommand } from '../../models/commands/save-parcel-template.command';
import { SavePackageTemplate } from '../../states/parcel-templates.action';

@Component({
    selector: 'app-parcel-template-definition-modal',
    templateUrl: './parcel-template-definition-modal.component.html',
    styleUrls: ['./parcel-template-definition-modal.component.scss'],
    standalone: false
})
export class ParcelTemplateDefinitionModalComponent implements OnDestroy {
  dialogRef = inject<MatDialogRef<ParcelTemplateDefinitionModalComponent>>(MatDialogRef);
  data = inject<ParcelTemplateDefinitionDataModel>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private store = inject(Store);

  form: FormGroup<TemplateParcelFormGroupModel>;
  subscriptions: Subscription = new Subscription();

  constructor() {
    const data = this.data;

    this.form = this.fb.group<TemplateParcelFormGroupModel>({
      parcels: this.fb.array<FormGroup>([], [Validators.required]),
      additionalInfo: new FormControl<string>(''),
    });

    if (data?.template?.parcels && data?.template?.parcels?.length > 0) {
      data.template?.parcels?.forEach(parcel => {
        this.addNewParcel(parcel?.length, parcel?.width, parcel?.height, parcel?.weight);
      });
    } else {
      this.addNewParcel();
    }

    this.form.markAllAsTouched();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.savePackageTemplate();
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

  get parcels(): FormArray<FormGroup> {
    return this.form.controls.parcels;
  }

  private savePackageTemplate() {
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

    const command: SaveParcelTemplateCommand = {
      tag: this.data.tag,
      parcelTemplates: parcels,
    };

    this.store.dispatch(new SavePackageTemplate(command));
  }
}
