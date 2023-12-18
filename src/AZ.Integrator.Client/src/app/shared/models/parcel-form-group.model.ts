import { FormControl } from '@angular/forms';

export interface ParcelFromGroupModel {
  length: FormControl<number | null>;
  width: FormControl<number | null>;
  height: FormControl<number | null>;
  weight: FormControl<number | null>;
}
