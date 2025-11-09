import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { SupplierMailboxFormGroup } from './supplier-mailbox-form-group.model';

export interface SupplierFormGroup {
  name: FormControl<string | null>;
  telephoneNumber: FormControl<string | null>;
  mailboxes: FormArray<FormGroup<SupplierMailboxFormGroup>>;
}
