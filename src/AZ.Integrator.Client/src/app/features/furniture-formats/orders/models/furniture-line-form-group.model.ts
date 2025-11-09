import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { PartLineFormGroup } from './part-line-form-group.model';

export interface FurnitureLineFormGroup {
  furnitureCode: FormControl<string | null>;
  furnitureVersion: FormControl<number | null>;
  quantityOrdered: FormControl<number | null>;
  partLines: FormArray<FormGroup<PartLineFormGroup>>;
}
