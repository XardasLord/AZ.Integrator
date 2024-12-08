import { FormArray, FormControl } from '@angular/forms';

export interface RegisterParcelFormGroupModel {
  receiverName: FormControl<string | null>;
  receiverCompanyName: FormControl<string | null>;
  receiverFirstName: FormControl<string | null>;
  receiverLastName: FormControl<string | null>;
  receiverEmail: FormControl<string | null>;
  receiverPhoneNumber: FormControl<string | null>;
  receiverAddressStreet: FormControl<string | null>;
  receiverAddressBuildingNumber: FormControl<string | null>;
  receiverAddressCity: FormControl<string | null>;
  receiverAddressPostCode: FormControl<string | null>;
  receiverAddressCountryCode: FormControl<string | null>;
  insuranceActive: FormControl<boolean | null>;
  insuranceAmount: FormControl<number | null>;
  insuranceCurrency: FormControl<string | null>;
  codActive: FormControl<boolean | null>;
  parcels: FormArray;
  additionalInfo: FormControl<string | null>;
}
