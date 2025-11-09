import { FormControl } from '@angular/forms';
import { EdgeBandingTypeViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

export interface PartLineFormGroup {
  partName: FormControl<string | null>;
  lengthMm: FormControl<number | null>;
  widthMm: FormControl<number | null>;
  thicknessMm: FormControl<number | null>;
  quantity: FormControl<number | null>;
  additionalInfo: FormControl<string | null>;
  lengthEdgeBandingType: FormControl<EdgeBandingTypeViewModel | null>;
  widthEdgeBandingType: FormControl<EdgeBandingTypeViewModel | null>;
}

export interface CreateOrderRequest {
  supplierId: number;
  furnitureLineRequests: CreateFurnitureLineRequest[];
  additionalNotes?: string;
}

export interface CreateFurnitureLineRequest {
  furnitureCode: string;
  furnitureVersion: number;
  quantityOrdered: number;
  partDefinitionLines: CreateFurniturePartLineRequest[];
}

export interface CreateFurniturePartLineRequest {
  partName: string;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  quantity: number;
  additionalInfo: string;
  lengthEdgeBandingType: number;
  widthEdgeBandingType: number;
}
