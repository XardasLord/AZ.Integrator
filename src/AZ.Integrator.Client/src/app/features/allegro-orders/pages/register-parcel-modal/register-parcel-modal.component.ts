import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { AllegroOrderDetailsModel } from '../../models/allegro-order-details.model';
import { CreateInpostShipmentCommand } from '../../models/commands/create-inpost-shipment.command';
import { ParcelFromGroupModel, RegisterParcelFormGroupModel } from '../../models/register-parcel-form-group.model';
import { RegisterInpostShipment } from '../../states/allegro-orders.action';

@Component({
  selector: 'app-register-parcel-modal',
  templateUrl: './register-parcel-modal.component.html',
  styleUrls: ['./register-parcel-modal.component.scss'],
})
export class RegisterParcelModalComponent {
  form: FormGroup<RegisterParcelFormGroupModel>;

  constructor(
    public dialogRef: MatDialogRef<RegisterParcelModalComponent>,
    @Inject(MAT_DIALOG_DATA) public allegroOrderDetails: AllegroOrderDetailsModel,
    private fb: FormBuilder,
    private store: Store
  ) {
    this.form = this.fb.group<RegisterParcelFormGroupModel>({
      receiverName: new FormControl<string>(allegroOrderDetails.buyer.login, [Validators.required]),
      receiverCompanyName: new FormControl<string>(allegroOrderDetails.buyer.companyName, []),
      receiverFirstName: new FormControl<string>(allegroOrderDetails.buyer.firstName, [Validators.required]),
      receiverLastName: new FormControl<string>(allegroOrderDetails.buyer.lastName, [Validators.required]),
      receiverEmail: new FormControl<string>(allegroOrderDetails.buyer.email, [Validators.required, Validators.email]),
      receiverPhoneNumber: new FormControl<string>(allegroOrderDetails.buyer.phoneNumber, [Validators.required]),
      receiverAddressStreet: new FormControl<string>(allegroOrderDetails.buyer.address.street, [Validators.required]),
      receiverAddressBuildingNumber: new FormControl<string>(allegroOrderDetails.buyer.address.street, [
        Validators.required,
      ]),
      receiverAddressCity: new FormControl<string>(allegroOrderDetails.buyer.address.city, [Validators.required]),
      receiverAddressPostCode: new FormControl<string>(allegroOrderDetails.buyer.address.postCode, [
        Validators.required,
      ]),
      receiverAddressCountryCode: new FormControl<string>(allegroOrderDetails.buyer.address.countryCode, [
        Validators.required,
      ]),
      insuranceAmount: new FormControl<number>(0, [Validators.required]),
      insuranceCurrency: new FormControl<string>('PLN', [Validators.required]),
      parcels: this.fb.array<FormGroup>([], [Validators.required]),
    });

    this.addNewParcel();
  }

  onSubmit() {
    if (this.form.valid) {
      const command: CreateInpostShipmentCommand = {
        receiver: {
          name: this.form.value.receiverName!,
          companyName: this.form.value.receiverCompanyName!,
          firstName: this.form.value.receiverFirstName!,
          lastName: this.form.value.receiverLastName!,
          email: this.form.value.receiverEmail!,
          phone: this.form.value.receiverPhoneNumber!,
          address: {
            street: this.form.value.receiverAddressStreet!,
            city: this.form.value.receiverAddressCity!,
            postCode: this.form.value.receiverAddressPostCode!,
            countryCode: this.form.value.receiverAddressCountryCode!,
            buildingNumber: '',
          },
        },
        sender: undefined!,
        parcels: [],
        insurance: {
          amount: this.form.value.insuranceAmount!,
          currency: this.form.value.insuranceCurrency!,
        },
        cod: undefined!,
        reference: this.allegroOrderDetails.id,
        comments: '',
        externalCustomerId: '',
      };

      this.store.dispatch(new RegisterInpostShipment(command));
    }
  addNewParcel() {
    const parcel = this.fb.group<ParcelFromGroupModel>({
      length: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
      width: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
      height: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
      weight: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
    });

    this.parcels.push(parcel);
  }

  removeParcel(index: number) {
    this.parcels.removeAt(index);
  }

  get parcels(): FormArray<FormGroup> {
    return this.form.controls.parcels;
  }
}
