import { FormArray, FormControl } from '@angular/forms';

export interface TemplatePackageFormGroupModel {
  tag: FormControl<string | null>;
  parcels: FormArray;
}
