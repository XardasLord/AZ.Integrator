import { FormArray, FormControl } from '@angular/forms';

export interface TemplateParcelFormGroupModel {
  parcels: FormArray;
  additionalInfo: FormControl<string | null>;
}
