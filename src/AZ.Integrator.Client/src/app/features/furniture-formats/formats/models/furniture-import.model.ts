export interface FurnitureImportData {
  furnitureCode: string;
  parts: PartImportData[];
}

export interface PartImportData {
  name: string;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  lengthEdgeBandingType: number;
  widthEdgeBandingType: number;
  quantity: number;
  additionalInfo: string;
}
