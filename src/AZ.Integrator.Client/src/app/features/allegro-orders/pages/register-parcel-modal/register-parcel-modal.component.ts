import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { AllegroOrderDetailsModel } from '../../models/allegro-order-details.model';
import { CreateInpostShipmentCommand, Parcel } from '../../models/commands/create-inpost-shipment.command';
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
      receiverPhoneNumber: new FormControl<string>(allegroOrderDetails.buyer.phoneNumber, [
        Validators.required,
        Validators.pattern('[0-9]{9}'),
      ]),
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
      insuranceActive: new FormControl<boolean>(false),
      insuranceAmount: new FormControl<number>({
        value: this.allegroOrderDetails.summary.totalToPay.amount,
        disabled: true,
      }),
      insuranceCurrency: new FormControl<string>({ value: 'PLN', disabled: true }),
      codActive: new FormControl<boolean>(false),
      parcels: this.fb.array<FormGroup>([], [Validators.required]),
    });

    this.addNewParcel();

    this.form.markAllAsTouched();

    this.form.controls.insuranceActive.valueChanges.subscribe(value => {
      if (value) {
        this.form.controls.insuranceAmount.setValidators(Validators.required);
        this.form.controls.insuranceAmount.enable();
        this.form.controls.insuranceCurrency.setValidators(Validators.required);
        this.form.controls.insuranceCurrency.enable();
      } else {
        this.form.controls.insuranceAmount.clearValidators();
        this.form.controls.insuranceAmount.disable();
        this.form.controls.insuranceCurrency.clearValidators();
        this.form.controls.insuranceCurrency.disable();
      }
    });

    this.form.controls.codActive.valueChanges.subscribe(value => {
      if (value) {
        this.form.controls.insuranceActive.setValue(true);
        this.form.controls.insuranceAmount.setValidators(Validators.required);
        this.form.controls.insuranceAmount.setValidators(Validators.min(this.form.controls.insuranceAmount.value!));
        this.form.controls.insuranceAmount.enable();
        this.form.controls.insuranceCurrency.setValidators(Validators.required);
        this.form.controls.insuranceCurrency.enable();
      } else {
        this.form.controls.insuranceAmount.clearValidators();
        this.form.controls.insuranceAmount.disable();
        this.form.controls.insuranceCurrency.clearValidators();
        this.form.controls.insuranceCurrency.disable();
      }
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    const parcels: Parcel[] = [];

    for (let i = 0; i < this.form.value.parcels.length; i++) {
      const parcel = this.form.value.parcels[i];

      parcels.push({
        id: `${i + 1}/${this.form.value.parcels.length}`,
        weight: {
          amount: parcel.weight.toString(),
          unit: 'kg',
        },
        dimensions: {
          length: parcel.length.toString(),
          width: parcel.width.toString(),
          height: parcel.height.toString(),
          unit: 'mm',
        },
        is_non_standard: false,
      });
    }

    const command: CreateInpostShipmentCommand = {
      receiver: {
        name: this.form.value.receiverName!,
        company_name: this.form.value.receiverCompanyName!,
        first_name: this.form.value.receiverFirstName!,
        last_name: this.form.value.receiverLastName!,
        email: this.form.value.receiverEmail!,
        phone: this.form.value.receiverPhoneNumber!,
        address: {
          street: this.form.value.receiverAddressStreet!,
          building_number: this.form.value.receiverAddressBuildingNumber!,
          city: this.form.value.receiverAddressCity!,
          post_code: this.form.value.receiverAddressPostCode!,
          country_code: this.form.value.receiverAddressCountryCode!,
        },
      },
      sender: undefined!,
      parcels: parcels,
      insurance: this.form.value.insuranceActive
        ? {
            amount: this.form.value.insuranceAmount!,
            currency: this.form.value.insuranceCurrency!,
          }
        : null,
      cod: this.form.value.codActive
        ? {
            amount: this.allegroOrderDetails.summary.totalToPay.amount!,
            currency: 'PLN',
          }
        : undefined!,
      reference: this.allegroOrderDetails.id,
      comments: '',
      external_customer_id: '',
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
