import { Component, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { map, Subscription } from 'rxjs';
import { CreateShipmentCommand, Parcel } from '../../models/commands/create-shipment.command';
import { RegisterParcelFormGroupModel } from '../../models/register-parcel-form-group.model';
import { RegisterDpdShipment, RegisterInpostShipment } from '../../states/allegro-orders.action';
import { RegisterShipmentDataModel } from '../../models/register-shipment-data.model';
import { AllegroOrdersService } from '../../services/allegro-orders.service';
import { ParcelFromGroupModel } from '../../../../shared/models/parcel-form-group.model';
import { GetTagParcelTemplatesGQL } from '../../../../shared/graphql/queries/get-tag-parcel-templates.graphql.query';
import { IntegratorQueryTagParcelTemplatesArgs } from '../../../../shared/graphql/graphql-integrator.schema';
import { AuthState } from '../../../../shared/states/auth.state';
import { GraphQLHelper } from '../../../../shared/graphql/graphql.helper';

@Component({
  selector: 'app-register-shipment-modal',
  templateUrl: './register-shipment-modal.component.html',
  styleUrls: ['./register-shipment-modal.component.scss'],
})
export class RegisterShipmentModalComponent implements OnDestroy {
  form: FormGroup<RegisterParcelFormGroupModel>;
  subscriptions: Subscription = new Subscription();

  constructor(
    public dialogRef: MatDialogRef<RegisterShipmentModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RegisterShipmentDataModel,
    private fb: FormBuilder,
    private store: Store,
    private allegroService: AllegroOrdersService,
    private tagParcelTemplatesGql: GetTagParcelTemplatesGQL
  ) {
    const allegroOrderDetails = data.allegroOrder;

    this.form = this.fb.group<RegisterParcelFormGroupModel>({
      receiverName: new FormControl<string>(allegroOrderDetails.buyer.login, [Validators.required]),
      receiverCompanyName: new FormControl<string>(allegroOrderDetails.buyer.companyName, []),
      receiverFirstName: new FormControl<string>(allegroOrderDetails.buyer.firstName, [Validators.required]),
      receiverLastName: new FormControl<string>(allegroOrderDetails.buyer.lastName, [Validators.required]),
      receiverEmail: new FormControl<string>(allegroOrderDetails.buyer.email, [Validators.required, Validators.email]),
      receiverPhoneNumber: new FormControl<string>(this.normalizePhoneNumber(allegroOrderDetails.buyer.phoneNumber), [
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
        value: allegroOrderDetails.summary.totalToPay.amount,
        disabled: true,
      }),
      insuranceCurrency: new FormControl<string>({ value: 'PLN', disabled: true }),
      codActive: new FormControl<boolean>(false),
      parcels: this.fb.array<FormGroup>([], [Validators.required]),
      additionalInfo: new FormControl<string>(''),
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

    if (allegroOrderDetails.payment.type === 'CASH_ON_DELIVERY') {
      this.form.controls.codActive.setValue(true);
    }

    this.subscriptions.add(
      this.allegroService.getOrderTags(data.allegroOrder.id).subscribe(tags => {
        this.form.controls.additionalInfo.setValue(tags.join(', '));

        if (tags.length === 0 || tags.length > 1) return;

        const query: IntegratorQueryTagParcelTemplatesArgs = {
          where: {
            tag: {
              eq: tags[0],
            },
            tenantId: {
              eq: this.store.selectSnapshot(AuthState.getUser)?.tenant_id,
            },
          },
        };

        this.tagParcelTemplatesGql
          .watch(query, GraphQLHelper.defaultGraphQLWatchQueryOptions)
          .valueChanges.pipe(map(x => x.data.result))
          .subscribe(results => {
            if (!results[0]) return;

            this.removeAllParcels();

            results[0].parcels?.forEach(parcel => {
              this.addNewParcel(parcel?.length, parcel?.width, parcel?.height, parcel?.weight);
            });
          });
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    if (this.data.deliveryMethodType === 'INPOST') {
      this.registerInpostShipment();
    } else if (this.data.deliveryMethodType === 'DPD') {
      this.registerDpdShipment();
    }
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

  removeAllParcels() {
    while (this.parcels.length !== 0) {
      this.removeParcel(0);
    }
  }

  get parcels(): FormArray<FormGroup> {
    return this.form.controls.parcels;
  }

  private registerInpostShipment() {
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

    const command: CreateShipmentCommand = {
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
            amount: this.data.allegroOrder.summary.totalToPay.amount!,
            currency: 'PLN',
          }
        : undefined!,
      reference: '',
      comments: this.form.value.additionalInfo!,
      external_customer_id: '',
      allegroOrderId: this.data.allegroOrder.id!,
    };

    this.store.dispatch(new RegisterInpostShipment(command));
  }

  private registerDpdShipment() {
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
          length: (parcel.length / 10).toString(),
          width: (parcel.width / 10).toString(),
          height: (parcel.height / 10).toString(),
          unit: 'cm',
        },
        is_non_standard: false,
      });
    }

    const command: CreateShipmentCommand = {
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
            amount: this.data.allegroOrder.summary.totalToPay.amount!,
            currency: 'PLN',
          }
        : undefined!,
      reference: '',
      comments: this.form.value.additionalInfo!,
      external_customer_id: '',
      allegroOrderId: this.data.allegroOrder.id!,
    };

    this.store.dispatch(new RegisterDpdShipment(command));
  }

  private normalizePhoneNumber(phoneNumber: string) {
    return phoneNumber.replace('+48', '').replace(/ /g, '');
  }
}
