import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { AllegroOrderDetailsModel } from '../../models/allegro-order-details.model';

@Component({
  selector: 'app-register-parcel-modal',
  templateUrl: './register-parcel-modal.component.html',
  styleUrls: ['./register-parcel-modal.component.scss'],
})
export class RegisterParcelModalComponent {
  form = this.fb.group({
    bankEmail: new FormControl<string>('', [Validators.required, Validators.email]),
  });

  constructor(
    public dialogRef: MatDialogRef<RegisterParcelModalComponent>,
    @Inject(MAT_DIALOG_DATA) public allegroOrderDetails: AllegroOrderDetailsModel,
    private fb: FormBuilder,
    private store: Store
  ) {}

  onSubmit() {
    if (this.form.valid) {
      // this.store.dispatch(new InviteBank(this.data.bankId, this.form.value.bankEmail ?? ''));
    }
  }
}
