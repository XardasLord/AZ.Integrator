import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { FurnitureLineFormGroup } from './furniture-line-form-group.model';

export interface OrderFormGroup {
  supplierId: FormControl<number | null>;
  furnitureLines: FormArray<FormGroup<FurnitureLineFormGroup>>;
}
