export interface ParcelTemplate {
  id: string;
  dimensions: DimensionTemplate;
  weight: number;
}

export interface DimensionTemplate {
  length: number;
  width: number;
  height: number;
}
