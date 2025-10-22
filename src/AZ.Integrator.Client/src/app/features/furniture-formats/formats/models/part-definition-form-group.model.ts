import { FormControl } from '@angular/forms';

export interface PartDefinitionFormGroup {
  id: FormControl<number | null>;
  name: FormControl<string | null>;
  lengthMm: FormControl<number | null>;
  widthMm: FormControl<number | null>;
  thicknessMm: FormControl<number | null>;
  color: FormControl<string | null>;
  additionalInfo: FormControl<string | null>;
}
