export interface ChangeStockQuantityCommand {
  packageCode: string;
  changeQuantity: number;
  scanId: string;
}
