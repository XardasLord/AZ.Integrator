import { FormControl } from '@angular/forms';
import { EdgeBandingTypeViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

export interface PartDefinitionFormGroup {
  id: FormControl<number | null>;
  name: FormControl<string | null>;
  lengthMm: FormControl<number | null>;
  widthMm: FormControl<number | null>;
  thicknessMm: FormControl<number | null>;
  lengthEdgeBandingType: FormControl<EdgeBandingTypeViewModel | null>;
  widthEdgeBandingType: FormControl<EdgeBandingTypeViewModel | null>;
  quantity: FormControl<number | null>;
  additionalInfo: FormControl<string | null>;
}
