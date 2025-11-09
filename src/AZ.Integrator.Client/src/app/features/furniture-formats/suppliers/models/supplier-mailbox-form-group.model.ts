import { FormControl, FormGroup } from '@angular/forms';

export interface SupplierMailboxFormGroup {
  id: FormControl<number | null>;
  email: FormControl<string | null>;
}

export type SupplierMailboxFormGroupType = FormGroup<SupplierMailboxFormGroup>;
