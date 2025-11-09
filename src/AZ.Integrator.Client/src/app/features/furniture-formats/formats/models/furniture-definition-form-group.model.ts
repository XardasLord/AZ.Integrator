import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { PartDefinitionFormGroup } from './part-definition-form-group.model';

export interface FurnitureDefinitionFormGroup {
  furnitureCode: FormControl<string | null>;
  partDefinitions: FormArray<FormGroup<PartDefinitionFormGroup>>;
}
