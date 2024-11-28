import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { map } from 'rxjs';
import { CreateShipmentCommand, Parcel } from '../../models/commands/create-shipment.command';
import { RegisterParcelFormGroupModel } from '../../models/register-parcel-form-group.model';
import { RegisterDpdShipment, RegisterInpostShipment } from '../../states/orders.action';
import { RegisterShipmentDataModel } from '../../models/register-shipment-data.model';
import { ParcelFromGroupModel } from '../../../../shared/models/parcel-form-group.model';
import { GetTagParcelTemplatesGQL } from '../../../../shared/graphql/queries/get-tag-parcel-templates.graphql.query';
import {
  IntegratorQueryTagParcelTemplatesArgs,
  TagParcelTemplateViewModel,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { GraphQLHelper } from '../../../../shared/graphql/graphql.helper';
import { OrderDetailsModel } from '../../models/order-details.model';

@Component({
  selector: 'app-register-shipment-modal',
  templateUrl: './register-shipment-modal.component.html',
  styleUrls: ['./register-shipment-modal.component.scss'],
})
export class RegisterShipmentModalComponent {
  form: FormGroup<RegisterParcelFormGroupModel>;

  constructor(
    public dialogRef: MatDialogRef<RegisterShipmentModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RegisterShipmentDataModel,
    private fb: FormBuilder,
    private store: Store,
    private tagParcelTemplatesGql: GetTagParcelTemplatesGQL
  ) {
    const orderDetails = data.order;

    this.form = this.fb.group<RegisterParcelFormGroupModel>({
      receiverName: new FormControl<string>(orderDetails.buyer.login, [Validators.required]),
      receiverCompanyName: new FormControl<string>(
        orderDetails.delivery?.address?.companyName ?? orderDetails.buyer.companyName,
        []
      ),
      receiverFirstName: new FormControl<string>(
        orderDetails.delivery?.address?.firstName ?? orderDetails.buyer.firstName,
        [Validators.required]
      ),
      receiverLastName: new FormControl<string>(
        orderDetails.delivery?.address?.lastName ?? orderDetails.buyer.lastName,
        [Validators.required]
      ),
      receiverEmail: new FormControl<string>(orderDetails.buyer.email, [Validators.required, Validators.email]),
      receiverPhoneNumber: new FormControl<string>(
        this.normalizePhoneNumber(orderDetails.delivery?.address?.phoneNumber ?? orderDetails.buyer.phoneNumber),
        [Validators.required, Validators.pattern('[0-9]{9}')]
      ),
      receiverAddressStreet: new FormControl<string>(
        this.extractStreetName(orderDetails.delivery?.address?.street) ??
          this.extractStreetName(orderDetails.delivery.address.street),
        [Validators.required]
      ),
      receiverAddressBuildingNumber: new FormControl<string>(
        this.extractBuildingNumber(orderDetails.delivery?.address?.street) ??
          this.extractBuildingNumber(orderDetails.delivery.address.street),
        [Validators.required]
      ),
      receiverAddressCity: new FormControl<string>(
        orderDetails.delivery?.address?.city ?? orderDetails.delivery.address.city,
        [Validators.required]
      ),
      receiverAddressPostCode: new FormControl<string>(
        orderDetails.delivery?.address?.zipCode ?? orderDetails.delivery.address.zipCode,
        [Validators.required]
      ),
      receiverAddressCountryCode: new FormControl<string>(
        orderDetails.delivery?.address?.countryCode ?? orderDetails.delivery.address.countryCode,
        [Validators.required]
      ),
      insuranceActive: new FormControl<boolean>(false),
      insuranceAmount: new FormControl<number>({
        value: orderDetails.summary.totalToPay.amount,
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

    if (orderDetails.payment.type === 'CASH_ON_DELIVERY') {
      this.form.controls.codActive.setValue(true);
    }

    this.getPredefinedParcelsForTag(orderDetails);
  }

  private getPredefinedParcelsForTag(orderDetails: OrderDetailsModel) {
    const tags = orderDetails.lineItems.filter(x => x.offer.external).map(x => x.offer.external?.id);

    this.form.controls.additionalInfo.setValue(tags.join(', '));

    if (tags.length === 0) return;

    const query: IntegratorQueryTagParcelTemplatesArgs = {
      where: {
        tag: {
          in: tags,
        },
      },
    };

    this.tagParcelTemplatesGql
      .watch(query, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data.result))
      .subscribe(results => {
        if (results.length < 1 && !results[0]) return;

        this.removeAllParcels();

        results.forEach((tagParcelTemplate: TagParcelTemplateViewModel) => {
          const totalQuantityOfBoughtProduct = orderDetails.lineItems
            .filter(x => x.offer.external?.id === tagParcelTemplate?.tag)
            .reduce((ty, u) => ty + u.quantity, 0);

          for (let i = 0; i < totalQuantityOfBoughtProduct; i++) {
            tagParcelTemplate?.parcels?.forEach(parcelTemplate => {
              this.addNewParcel(
                parcelTemplate?.length,
                parcelTemplate?.width,
                parcelTemplate?.height,
                parcelTemplate?.weight
              );
            });
          }
        });
      });
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
            amount: this.data.order.summary.totalToPay.amount!,
            currency: 'PLN',
          }
        : undefined!,
      reference: '',
      comments: this.form.value.additionalInfo!,
      external_customer_id: '',
      externalOrderId: this.data.order.id!,
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
            amount: this.data.order.summary.totalToPay.amount!,
            currency: 'PLN',
          }
        : undefined!,
      reference: '',
      comments: this.form.value.additionalInfo!,
      external_customer_id: '',
      externalOrderId: this.data.order.id!,
    };

    this.store.dispatch(new RegisterDpdShipment(command));
  }

  private normalizePhoneNumber(phoneNumber: string) {
    return phoneNumber.replace('+48', '').replace(/ /g, '');
  }

  private extractStreetName(address: string): string {
    const regex = /^(.*?)(?=\s+\d[\w\/]*$)/;
    const match = address.match(regex);
    return match ? match[1].trim() : address;
  }

  private extractBuildingNumber(address: string): string {
    const regex = /\s+\d[\w\/]*$/;
    const match = address.match(regex);

    return match ? match[0] : '';
  }
}
