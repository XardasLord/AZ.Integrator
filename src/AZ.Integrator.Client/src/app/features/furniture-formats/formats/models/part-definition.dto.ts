export interface PartDefinitionDto {
  id?: number | null;
  name: string;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  quantity: number;
  additionalInfo: string;
}
